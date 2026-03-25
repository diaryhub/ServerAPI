using Microsoft.AspNetCore.Mvc;
using ServerAPI.DTOs.Request;
using ServerAPI.DTOs.Response;
using ServerAPI.Services.Interfaces;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var token = await authService.LoginAsync(request.Email, request.Password);
            if (token == null)
                return Unauthorized(new { message = "이메일 또는 비밀번호가 올바르지 않습니다." });

            return Ok(new LoginResponseDto { Token = token });
        }
    }
}
