using ServerAPI.DTOs.Request;
using ServerAPI.DTOs.Response;

namespace ServerAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto?> RegisterUserAsync(RegisterRequestDto request);
        Task<UserResponseDto?> GetUserByIdAsync(int userId);
        Task<bool> IsEmailTakenAsync(string email);
    }
}
