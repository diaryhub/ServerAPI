using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.DTOs.Request;
using ServerAPI.DTOs.Response;
using ServerAPI.Services.Interfaces;
using System.Security.Claims;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GachaController(IGachaService gachaService) : ControllerBase
    {
        [Authorize]
        [HttpPost("draw")]
        public async Task<IActionResult> DrawGacha([FromBody] GachaRequestDto request)
        {
            string? userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                return Unauthorized(new { message = "잘못된 토큰 정보입니다." });

            try
            {
                var result = await gachaService.DrawGachaAsync(userId, request.BannerId);

                if (!result.Success)
                    return BadRequest(new { message = result.Message });

                return Ok(new GachaResponseDto
                {
                    Message = result.Message,
                    BannerName = result.BannerName ?? string.Empty,
                    ItemId = result.ItemId ?? 0,
                    Grade = result.Grade ?? 0,
                    IsPickup = result.IsPickup ?? false
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "서버 내부 오류로 인해 가챠 처리에 실패했습니다." });
            }
        }
    }
}
