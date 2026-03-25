namespace ServerAPI.Services.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(int userId, string nickname = "");
        Task<string?> LoginAsync(string email, string password);
    }
}
