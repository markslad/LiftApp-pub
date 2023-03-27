using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Dal.Models
{
    public class Address
    {
        public int Id { get; set; } = default!;

        public string Street { get; set; } = default!;

        public int HouseNumber { get; set; } = default!;

        public string City { get; set; } = default!;

        public string ZipCode { get; set; } = default!;

        public string Country { get; set; } = default!;

        public Office? Office { get; set; }

        public Customer? Customer { get; set; }
    }
}
