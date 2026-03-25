using System.ComponentModel.DataAnnotations;

namespace ServerAPI.DTOs.Request
{
    public class RegisterRequestDto
    {
        [Required]
        [MaxLength(20, ErrorMessage = "닉네임은 20자 이하여야 합니다.")]
        public string Nickname { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "올바른 이메일 형식이 아닙니다.")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "비밀번호는 8자 이상이어야 합니다.")]
        public string Password { get; set; } = string.Empty;
    }
}
