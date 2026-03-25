using Microsoft.AspNetCore.Mvc;
using ServerAPI.Services.Interfaces;

namespace ServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LauncherController(ILauncherService launcherService) : ControllerBase
    {
        [HttpGet("notices")]
        public async Task<IActionResult> GetNotices()
        {
            var notices = await launcherService.GetNoticesAsync();
            return Ok(notices);
        }

        [HttpGet("server-status")]
        public async Task<IActionResult> GetServerStatus()
        {
            var status = await launcherService.GetServerStatusAsync();
            return Ok(new { status });
        }

        [HttpGet("version")]
        public async Task<IActionResult> GetVersion()
        {
            var version = await launcherService.GetVersionAsync();
            return Ok(version);
        }
    }
}
