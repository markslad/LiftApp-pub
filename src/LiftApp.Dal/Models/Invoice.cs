using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Dal.Models
{
    public class Invoice
    {
        public string Identifier { get; set; } = default!;

        public DateOnly DateOfIssue { get; set; } = default!;

        public DateOnly DueDate { get; set; } = default!;

        public DateOnly DateOfTaxableSupply { get; set; } = default!;

        public int Price { get; set; }

        public float ValueAddedTaxRate { get; set; }

        public bool Paid { get; set; }

        public string Bank { get; set; } = default!;

        public string BankAccount { get; set; } = default!;

        public string VariableSymbol { get; set; } = default!;

        public int BorrowalId { get; set; } = default!;

        public Borrowal Borrowal { get; set; } = default!;

        public string? Description { get; set; }

        public bool IsExtra { get; set; } = default!;
    }
}
