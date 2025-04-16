using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class Seasons
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }

        public ICollection<PlantingPlans>? PlantingPlans { get; set; }
    }
}