using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Fertilizer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public decimal NitrogenContent { get; set; }

        [Required]
        public decimal PhosphorusContent { get; set; }

        [Required]
        public decimal PotassiumContent { get; set; }

        [Required]
        public decimal PricePerUnit { get; set; }

        [Required]
        public string Unit { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}