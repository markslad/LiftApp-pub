using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Options
{
    public class NewInvoiceOptions
    {
        public float ValueAddedTaxRate { get; set; } = default!;

        public float ExtraInvoiceValueAddedTaxRate { get; set; } = default!;

        public string Bank { get; set; } = default!;
        
        public string BankAccount { get; set; } = default!;

        public int NumberOfDaysAfterDateOfIssueForDueDate { get; set; }
    }
}
