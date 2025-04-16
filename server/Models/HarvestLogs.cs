using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class HarvestLogs
    {
        public Guid Id { get; set; }
        public double Actual_yield { get; set; }
        public DateTime Harvest_date { get; set; }
        public int Quality_rating { get; set; }

        //FK
        public Guid Plan_Id { get; set; }
        public PlantingPlans? PlantingPlans { get; set; }
    }
}