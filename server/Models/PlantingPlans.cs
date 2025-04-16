using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class PlantingPlans
    {
        public Guid Id { get; set; }
        public DateTime Planned_date { get; set; }
        public double Expected_yield { get; set; }

        //FK
        public Guid Field_id { get; set; }
        public Fields? Fields { get; set; }
        public Guid Crop_id { get; set; }
        public Crops? Crops { get; set; }
        public Guid Season_id { get; set; }
        public Seasons? Seasons { get; set; }

        public ICollection<FertilizationPlans>? FertilizationPlans { get; set; }
        public HarvestLogs? HarvestLogs { get; set; }
        public ICollection<Tasks>? Tasks { get; set; }
    }
}