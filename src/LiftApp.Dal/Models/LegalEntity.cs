using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Dal.Models
{
    public class LegalEntity : EntrepreneurCustomer
    {
        public string Name { get; set; } = default!;
    }
}
