using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerApi.Data;
using ServerApi.Models;

namespace ServerAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController(AppDbContext context) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] string nickname) {

            if (await context.Users.AnyAsync(u => u.Nickname == nickname)) {
                return BadRequest("이미 존재하는 닉네임 입니다.");
            }

            var newUser = new User
            {
                Nickname = nickname,
                Currency = 1000
            };

            context.Users.Add(newUser);
            await context.SaveChangesAsync();

            return Ok(newUser);  
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id) { 
            var user = await context.Users.FindAsync(id);

            if (user == null) {
                return NotFound("사용자를 찾을 수 없습니다.");
            }

            return Ok(user);
        }
    }
}
