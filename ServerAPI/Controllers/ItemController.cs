using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApi.Data;
using ServerApi.Models;

namespace ServerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ItemController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("gacha/{userId}")]
        public async Task<IActionResult> RollGacha(int userId)
        {
            // 1. 유저 존재 여부 검증
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("해당 유저를 찾을 수 없습니다.");

            // 2. 전체 아이템 목록 조회 (현재는 DB에서 직접 조회하지만, 실무에선 메모리에 캐싱함)
            var items = await _context.Items.ToListAsync();
            if (items.Count < 1)
                return BadRequest("서버에 등록된 아이템이 없습니다.");

            // 3. 무작위 아이템 추첨 (균등 확률)
            var random = new Random();
            var selectedItem = items[random.Next(items.Count)];

            // 4. 유저 인벤토리 확인 (보유 여부에 따라 INSERT 또는 UPDATE)
            var inventoryItem = await _context.UserInventories
                .FirstOrDefaultAsync(i => i.UserId == userId && i.ItemId == selectedItem.Id);

            if (inventoryItem != null)
            {
                // 이미 보유한 아이템이면 수량(Quantity) 1 증가
                inventoryItem.Quantity += 1;
            }
            else
            {
                // 미보유 아이템이면 인벤토리에 신규 데이터 생성
                _context.UserInventories.Add(new UserInventory
                {
                    UserId = userId,
                    ItemId = selectedItem.Id,
                    Quantity = 1
                });
            }

            // 5. 변경사항 DB 저장
            await _context.SaveChangesAsync();

            // 6. 결과 반환
            return Ok(new
            {
                Message = "가챠 성공!",
                AcquiredItem = selectedItem.Name,
                ItemId = selectedItem.Id
            });
        }
    }
}