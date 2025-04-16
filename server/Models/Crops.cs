using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Crops
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Growth_period { get; set; }
        public string Water_requirements { get; set; } = string.Empty;
        public double Optimal_temperature { get; set; }

        public ICollection<PlantingPlans>? PlantingPlans { get; set; }
    }
}
