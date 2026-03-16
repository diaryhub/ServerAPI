using System.ComponentModel.DataAnnotations;

namespace ServerAPI.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string ItemType { get; set; }

        public string Description { get; set; }
    }
}
