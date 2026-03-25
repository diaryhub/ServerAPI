using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerAPI.Models
{
    // 1. 신규 배너 마스터 테이블
    [Table("gacha_banners")]
    public class GachaBanner
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } // 배너명
        public DateTime StartTime { get; set; } // 시작 시간
        public DateTime EndTime { get; set; }   // 종료 시간
        public int Cost { get; set; }
    }

    [Table("gacha_rates")]
    public class GachaRate
    {
        [Key]
        public int Id { get; set; }
        public int BannerId { get; set; }
        public int ItemId { get; set; }
        public int Grade { get; set; }
        public int Weight { get; set; }
        public bool IsPickup { get; set; }
    }

    [Table("gacha_logs")]
    public class GachaLog
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
