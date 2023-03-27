using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftApp.Dal.Enums;

namespace LiftApp.Dal.Models
{
    public class Lift
    {
        public string SerialNumber { get; set; } = default!;

        public string Manufacturer { get; set; } = default!;

        public int MaximumHeight { get; set; } = default!;

        public PowerSource PowerSource { get; set; } = default!;

        public bool Eliminated { get; set; } = default!;

        public int OfficeId { get; set; }

        public Office Office { get; set; } = default!;

        public List<Maintenance> Maintenances { get; set; } = default!;

        public List<Borrowal> Borrowals { get; set; } = default!;
    }
}
