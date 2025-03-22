using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Crop
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string ScientificName { get; set; }

        [Required]
        public int GrowingSeason { get; set; }

        [Required]
        public decimal AverageYield { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}