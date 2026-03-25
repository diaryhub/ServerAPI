namespace ServerAPI.DTOs.Response
{
    public class VersionResponseDto
    {
        public string Version { get; set; } = string.Empty;
        public string PatchNote { get; set; } = string.Empty;
        public string ReleaseDate { get; set; } = string.Empty;
    }
}
