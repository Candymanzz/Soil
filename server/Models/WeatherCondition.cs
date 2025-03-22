using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class WeatherCondition
    {
        public int Id { get; set; }

        [Required]
        public int FieldId { get; set; }
        public Field Field { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Temperature { get; set; }

        [Required]
        public decimal Humidity { get; set; }

        [Required]
        public decimal Precipitation { get; set; }

        [Required]
        public decimal WindSpeed { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}