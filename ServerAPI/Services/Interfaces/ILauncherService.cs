using ServerAPI.DTOs.Response;

namespace ServerAPI.Services.Interfaces
{
    public interface ILauncherService
    {
        Task<IEnumerable<NoticeResponseDto>> GetNoticesAsync();
        Task<string> GetServerStatusAsync();
        Task<VersionResponseDto> GetVersionAsync();
        Task<IEnumerable<BannerResponseDto>> GetBannersAsync();
    }
}
