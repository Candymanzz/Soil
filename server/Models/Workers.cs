using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server.Models
{
    public class Workers
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;

        public ICollection<Tasks>? Tasks { get; set; }
    }
}