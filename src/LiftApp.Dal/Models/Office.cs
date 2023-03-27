using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Dal.Models
{
    public class Office
    {
        public int Id { get; set; } = default!;

        public int AddressId { get; set; } = default!;

        public Address Address { get; set; } = default!;

        public List<Lift> Lifts { get; set; } = default!;
    }
}
