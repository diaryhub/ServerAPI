namespace ServerAPI.DTOs.Response
{
    public class GachaResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public string BannerName { get; set; } = string.Empty;
        public int ItemId { get; set; }
        public int Grade { get; set; }
        public bool IsPickup { get; set; }
    }
}
