using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Dal.Models
{
    public class Borrowal
    {
        public int Id { get; set; } = default!;

        public int PriceADay { get; set; } = default!;

        public int TimeIntervalId { get; set; } = default!;

        public TimeInterval TimeInterval { get; set; } = default!;

        public string LiftSerialNumber { get; set; } = default!;

        public Lift Lift { get; set; } = default!;

        public List<Invoice> Invoices { get; set; } = default!;

        public string CustomerIdentifier { get; set; } = default!;

        public Customer Customer { get; set; } = default!;
    }
}
