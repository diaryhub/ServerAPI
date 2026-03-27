using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerApi.Models
{
    [Table("game_versions")]
    public class GameVersion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Version { get; set; } = string.Empty;

        [Required]
        public string PatchNote { get; set; } = string.Empty;

        public DateOnly ReleaseDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
