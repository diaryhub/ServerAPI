using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.DTOs.Request;
using ServerAPI.Services.Interfaces;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDto request)
        {
            var user = await userService.RegisterUserAsync(request);
            if (user == null)
                return BadRequest(new { message = "이미 사용 중인 이메일입니다." });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpGet("check-email")]
        public async Task<IActionResult> CheckEmail([FromQuery] string email)
        {
            var taken = await userService.IsEmailTakenAsync(email);
            return Ok(new { taken });
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "사용자를 찾을 수 없습니다." });

            return Ok(user);
        }
    }
}
