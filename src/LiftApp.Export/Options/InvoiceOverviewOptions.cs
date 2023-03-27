using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Options
{
    public class InvoiceOverviewOptions
    {
        public int ExportDateColumnIndex { get; set; } = default!;
        public int ExportDateRowIndex { get; set; } = default!;
        public int DateRangeColumnIndex { get; set; } = default!;
        public int DateRangeRowIndex { get; set; } = default!;
        public int StartRowIndex { get; set; } = default!;

        public int InvoiceIdentifierColumnIndex { get; set; } = default!;
        public int DateOfIssueColumnIndex { get; set; } = default!;
        public int DueDateColumnIndex { get; set; } = default!;
        public int DateOfTaxableSupplyColumnIndex { get; set; } = default!;
        public int PriceColumnIndex { get; set; } = default!;
        public int ValueAddedTaxRateColumnIndex { get; set; } = default!;
        public int PriceWithValueAddedTaxColumnIndex { get; set; } = default!;
        public int BankColumnIndex { get; set; } = default!;
        public int BankAccountColumnIndex { get; set; } = default!;
        public int VariableSymbolColumnIndex { get; set; } = default!;
        public int IsExtraColumnIndex { get; set; } = default!;
        public int PaidColumnIndex { get; set; } = default!;
        public int DescriptionColumnIndex { get; set; } = default!;

        public int BorrowalIDColumnIndex { get; set; } = default!;
        public int BorrowalDateFromColumnIndex { get; set; } = default!;
        public int BorrowalDateToColumnIndex { get; set; } = default!;
        public int BorrowalPriceADayColumnIndex { get; set; } = default!;


        public int LiftIdentifierColumnIndex { get; set; } = default!;

        public int CustomerIdentifierColumnIndex { get; set; } = default!;
    }
}
