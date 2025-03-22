using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class FieldAnalysis
    {
        public int Id { get; set; }

        [Required]
        public int FieldId { get; set; }
        public Field Field { get; set; }

        [Required]
        public DateTime AnalysisDate { get; set; }

        [Required]
        public decimal pH { get; set; }

        [Required]
        public decimal Nitrogen { get; set; }

        [Required]
        public decimal Phosphorus { get; set; }

        [Required]
        public decimal Potassium { get; set; }

        [Required]
        public decimal OrganicMatter { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}