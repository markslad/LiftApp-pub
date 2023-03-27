using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Dal.Models
{
    public class Maintenance
    {
        public int Id { get; set; }

        public string Description { get; set; } = default!;

        public int? Price { get; set; } = default!;

        public int TimeIntervalId { get; set; } = default!;

        public TimeInterval TimeInterval { get; set; } = default!;

        public string LiftSerialNumber { get; set; } = default!;

        public Lift Lift { get; set; } = default!;
    }
}
