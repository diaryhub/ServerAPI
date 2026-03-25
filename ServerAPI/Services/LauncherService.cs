using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

using ServerApi.Data;
using ServerAPI.DTOs.Response;
using ServerAPI.Services.Interfaces;

namespace ServerAPI.Services
{
    public class LauncherService(AppDbContext db, IDistributedCache cache, IConfiguration configuration) : ILauncherService
    {
        public async Task<IEnumerable<NoticeResponseDto>> GetNoticesAsync()
        {
            const string cacheKey = "launcher_notices";

            var cached = await cache.GetStringAsync(cacheKey);
            if (cached != null)
                return JsonSerializer.Deserialize<IEnumerable<NoticeResponseDto>>(cached)!;

            var notices = await db.Notices
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new NoticeResponseDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Content = n.Content,
                    CreatedAt = n.CreatedAt.ToString("yyyy-MM-dd")
                })
                .ToListAsync();

            await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(notices), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            return notices;
        }

        public async Task<string> GetServerStatusAsync()
        {
            const string cacheKey = "launcher_server_status";

            var cached = await cache.GetStringAsync(cacheKey);
            if (cached != null)
                return cached;

            var status = configuration["Launcher:ServerStatus"] ?? "online";

            await cache.SetStringAsync(cacheKey, status, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
            });

            return status;
        }

        public async Task<VersionResponseDto> GetVersionAsync()
        {
            const string cacheKey = "launcher_version";

            var cached = await cache.GetStringAsync(cacheKey);
            if (cached != null)
                return JsonSerializer.Deserialize<VersionResponseDto>(cached)!;

            var version = new VersionResponseDto
            {
                Version = configuration["Launcher:Version"] ?? "1.0.0",
                PatchNote = configuration["Launcher:PatchNote"] ?? "",
                ReleaseDate = configuration["Launcher:ReleaseDate"] ?? ""
            };

            await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(version), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            });

            return version;
        }
    }
}
