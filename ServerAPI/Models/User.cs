using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerApi.Models
{
    // DB의 'Users' 테이블과 1:1 매핑될 엔티티(Entity) 클래스
    public class User
    {
        [Key] // Primary Key 설정
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto Increment (자동 증가)
        public int Id { get; set; }

        [Required] // NOT NULL 설정
        [MaxLength(50)] // VARCHAR(50) 크기 제한
        public string Nickname { get; set; } = string.Empty;

        // 재화
        public int Currency { get; set; } = 0;

        // 계정 생성일
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}