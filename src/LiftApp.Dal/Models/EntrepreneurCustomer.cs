using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Dal.Models
{
    public abstract class EntrepreneurCustomer : Customer
    {
        public string? TaxIdentificationNumber { get; set; }
    }
}
