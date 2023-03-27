using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Dal.Models
{
    public class TimeInterval
    {
        public int Id { get; set; }

        public DateOnly DateFrom { get; set; }

        public DateOnly DateTo { get; set; }

        public Maintenance? Maintenance { get; set; }

        public Borrowal? Borrowal { get; set; }
    }
}
