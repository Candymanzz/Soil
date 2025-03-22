using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class FertilizationPlan
    {
        public int Id { get; set; }

        [Required]
        public int FieldId { get; set; }
        public Field Field { get; set; }

        [Required]
        public int CropId { get; set; }
        public Crop Crop { get; set; }

        [Required]
        public DateTime PlannedDate { get; set; }

        [Required]
        public int FertilizerId { get; set; }
        public Fertilizer Fertilizer { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string ApplicationMethod { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}