using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ServerApi.Data;
using ServerApi.Models;
using ServerAPI.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GachaController(AppDbContext context, IDistributedCache cache) : ControllerBase
    {
        [Authorize]
        [HttpPost("draw")]
        public async Task<IActionResult> DrawTrickcalGacha([FromBody] GachaRequest request)
        {

            string userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized(new { message = "잘못된 토큰 정보입니다." });
            }

            // 1. 트랜잭션 시작 (재화 차감과 아이템 지급의 원자성 보장)
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // 1. 배너 유효기간 1차 검증
                DateTime currentTime = DateTime.UtcNow;
                var banner = await context.GachaBanners.FindAsync(request.BannerId);

                if (banner == null || currentTime < banner.StartTime || currentTime > banner.EndTime)
                {
                    return BadRequest(new { message = "존재하지 않거나 현재 진행 중이 아닌 배너입니다." });
                }

                // 2. 유저 정보 및 재화 검증
                var user = await context.Users.FindAsync(userId);
                if (user == null || user.Currency < banner.Cost)
                {
                    return BadRequest(new { message = "유저를 찾을 수 없거나 재화가 부족합니다." });
                }

                // 3. 재화 차감
                user.Currency -= banner.Cost;

                // 4. Redis 캐싱을 적용한 확률 데이터 로드 (Cache-Aside 패턴)
                string cacheKey = $"banner_rates_{request.BannerId}";
                List<GachaRate> rates = null;

                // 4-1. Redis에서 캐시 데이터 조회 시도
                string cachedRates = await cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedRates))
                {
                    // [Cache Hit] Redis에 데이터가 있으면 DB를 거치지 않고 바로 역직렬화하여 사용
                    rates = JsonSerializer.Deserialize<List<GachaRate>>(cachedRates);
                }
                else
                {
                    // [Cache Miss] Redis에 데이터가 없으면 PostgreSQL에서 직접 조회
                    rates = await context.GachaRates
                        .Where(r => r.BannerId == request.BannerId)
                        .ToListAsync();

                    if (rates.Count > 0)
                    {
                        // 다음 요청을 위해 조회한 데이터를 직렬화하여 Redis에 저장 (유효기간: 1시간)
                        var cacheOptions = new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                        };
                        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(rates), cacheOptions);
                    }
                }

                // 정합성 검증은 캐시에서 가져오든 DB에서 가져오든 동일하게 수행
                int totalWeight = rates.Sum(r => r.Weight);
                if (totalWeight != 1000)
                {
                    throw new Exception($"데이터 정합성 오류: 배너 가중치 총합이 1000이어야 하나, 현재 {totalWeight}입니다.");
                }

                // 5. 난수 추첨 로직
                int randomValue = RandomNumberGenerator.GetInt32(0, 1000);
                int currentWeight = 0;
                GachaRate winItem = null;

                foreach (var rate in rates)
                {
                    currentWeight += rate.Weight;
                    if (randomValue < currentWeight)
                    {
                        winItem = rate;
                        break;
                    }
                }

                if (winItem == null) throw new Exception("추첨 로직 실패");

                // 6. 인벤토리 지급 및 로그 기록
                var newInventory = new UserInventory
                {
                    UserId = user.Id,
                    ItemId = winItem.ItemId,
                    ObtainedAt = DateTime.UtcNow
                };
                context.UserInventories.Add(newInventory);

                var gachaLog = new GachaLog
                {
                    UserId = user.Id,
                    ItemId = winItem.ItemId,
                    CreatedAt = DateTime.UtcNow
                };
                context.GachaLogs.Add(gachaLog);

                // 7. 트랜잭션 커밋
                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "가챠 성공",
                    bannerName = banner.Name,
                    itemId = winItem.ItemId,
                    grade = winItem.Grade,
                    isPickup = winItem.IsPickup
                });
            }
            catch (Exception ex)
            {
                // 9. 예외 발생 시 모든 작업을 취소하고 롤백
                await transaction.RollbackAsync();
                return StatusCode(500, new { message = "서버 내부 오류로 인해 결제가 취소되었습니다." });
            }
        }
    }

}
