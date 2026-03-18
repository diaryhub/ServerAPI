using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ServerAPI.Models;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.UserId <= 0)
            {
                return BadRequest(new { message = "유효하지 않은 유저 ID입니다." });
            }

            // 1. Payload(Claim) 설정: 토큰 내부에 유저 식별자(UserId)를 숨겨 넣습니다.
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, request.UserId.ToString())
        };

            // 2. Signature(서명) 설정: appsettings.json의 비밀키로 서버 도장을 찍습니다.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 3. 토큰 객체 생성 (유효기간 1시간)
            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            // 4. 문자열로 직렬화하여 클라이언트에 반환
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
    }

}
