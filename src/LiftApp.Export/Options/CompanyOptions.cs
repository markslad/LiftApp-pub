using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Options
{
    public class CompanyOptions
    {
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Identifier { get; set; } = default!;
        public string TaxIdentificationNumber { get; set; } = default!;
    }
}
