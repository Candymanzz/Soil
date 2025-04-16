using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class FertilizationPlans
    {
        public Guid Id { get; set; }
        public DateTime Application_date { get; set; }
        public int Amount { get; set; }

        //FK
        public Guid Plan_Id { get; set; }
        public PlantingPlans? PlantingPlans { get; set; }
        public Guid Fertilization_Id { get; set; }
        public Fertilizers? Fertilizers { get; set; }
    }
}