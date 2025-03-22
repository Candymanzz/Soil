using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Field
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public decimal Area { get; set; }

        [Required]
        public int SoilTypeId { get; set; }
        public SoilType SoilType { get; set; }

        [Required]
        public string Location { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}