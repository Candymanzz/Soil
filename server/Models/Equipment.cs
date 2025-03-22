using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Equipment
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public decimal PurchasePrice { get; set; }

        public string SerialNumber { get; set; }

        public string Status { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}