using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class Tasks
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public string Status { get; set; } = string.Empty;

        //FK M:M
        public ICollection<PlantingPlans>? PlantingPlans { get; set; }
        public ICollection<Workers>? Workers { get; set; }
        public ICollection<Equipment>? Equipment { get; set; }
    }
}