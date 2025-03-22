using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class FertilizationHistory
    {
        public int Id { get; set; }

        [Required]
        public int FieldId { get; set; }
        public Field Field { get; set; }

        [Required]
        public int FertilizerId { get; set; }
        public Fertilizer Fertilizer { get; set; }

        [Required]
        public DateTime ApplicationDate { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string ApplicationMethod { get; set; }

        [Required]
        public int UserId { get; set; }
        public User User { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}