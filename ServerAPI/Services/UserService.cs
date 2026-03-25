using Microsoft.EntityFrameworkCore;
using ServerApi.Data;
using ServerApi.Models;
using ServerAPI.DTOs.Request;
using ServerAPI.DTOs.Response;
using ServerAPI.Services.Interfaces;

namespace ServerAPI.Services
{
    public class UserService(AppDbContext context) : IUserService
    {
        public async Task<UserResponseDto?> RegisterUserAsync(RegisterRequestDto request)
        {
            if (await context.Users.AnyAsync(u => u.Email == request.Email))
                return null;

            var newUser = new User
            {
                Nickname = request.Nickname,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Currency = 1000
            };

            context.Users.Add(newUser);
            await context.SaveChangesAsync();

            return ToDto(newUser);
        }

        public async Task<bool> IsEmailTakenAsync(string email) =>
            await context.Users.AnyAsync(u => u.Email == email);

        public async Task<UserResponseDto?> GetUserByIdAsync(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            return user == null ? null : ToDto(user);
        }

        private static UserResponseDto ToDto(User user) => new()
        {
            Id = user.Id,
            Nickname = user.Nickname,
            Email = user.Email,
            Currency = user.Currency,
            CreatedAt = user.CreatedAt
        };
    }
}
