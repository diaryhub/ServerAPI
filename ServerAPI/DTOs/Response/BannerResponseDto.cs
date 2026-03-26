namespace ServerAPI.DTOs.Response
{
    public class BannerResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public int Cost { get; set; }
        public string? ImageUrl { get; set; }
    }
}
