namespace ServerAPI.DTOs.Response
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Currency { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
