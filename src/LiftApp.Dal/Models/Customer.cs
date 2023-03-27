using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Dal.Models
{
    public abstract class Customer
    {
        public string Identifier { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;

        public string Email { get; set; } = default!;

        public int AddressId { get; set; } = default!;

        public Address Address { get; set; } = default!;

        public List<Borrowal> Borrowals { get; set; } = default!;
    }
}
