using ServerAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerApi.Models
{
    public class UserInventory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ItemId { get; set; }

        public int Quantity { get; set; }

        // EF Core 테이블 조인(Join)을 위한 내비게이션 속성
        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("ItemId")]
        public Item Item { get; set; }
    }
}