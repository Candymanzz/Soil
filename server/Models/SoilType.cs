using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class SoilType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal pH { get; set; }

        [Required]
        public decimal OrganicMatter { get; set; }

        [Required]
        public decimal SandContent { get; set; }

        [Required]
        public decimal SiltContent { get; set; }

        [Required]
        public decimal ClayContent { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}