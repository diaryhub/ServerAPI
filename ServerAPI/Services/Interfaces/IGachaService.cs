using ServerApi.Models;

namespace ServerAPI.Services.Interfaces
{
    public interface IGachaService
    {
        Task<GachaResult> DrawGachaAsync(int userId, int bannerId);
    }

    public class GachaResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? BannerName { get; set; }
        public int? ItemId { get; set; }
        public int? Grade { get; set; }
        public bool? IsPickup { get; set; }
    }
}
