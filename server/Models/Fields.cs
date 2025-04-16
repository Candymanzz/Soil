using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models
{
    public class Fields
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public double Area { get; set; }
        public string Soil_type { get; set; } = string.Empty;
        public string Coordinates { get; set; } = string.Empty;

        public ICollection<PlantingPlans>? PlantingPlans { get; set; }
    }
}
