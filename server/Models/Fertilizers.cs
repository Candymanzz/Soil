using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class Fertilizers
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Composition { get; set; } = string.Empty;

        public ICollection<FertilizationPlans>? FertilizationPlans { get; set; }
    }
}
