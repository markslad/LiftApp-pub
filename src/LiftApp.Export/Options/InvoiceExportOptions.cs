using LiftApp.Export.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Options
{
    public class InvoiceExportOptions
    {
        public string ExcelTemplatePath { get; set; } = default!;
        public RowColumn InvoiceIdentifier { get; set; } = default!;
        public RowColumn ContractorName { get; set; } = default!;
        public RowColumn ContractorAddress { get; set; } = default!;
        public RowColumn ContractorIdentifier { get; set; } = default!;
        public RowColumn ContractorTaxIdentificationNumber { get; set; } = default!;
        public RowColumn ContractorBank { get; set; } = default!;
        public RowColumn ContractorBankAccount { get; set; } = default!;
        public RowColumn ContractorVariableSymbol { get; set; } = default!;
        public RowColumn CustomerName { get; set; } = default!;
        public RowColumn CustomerAddress { get; set; } = default!;
        public RowColumn CustomerIdentifier { get; set; } = default!;
        public RowColumn CustomerTaxIdentificationNumber { get; set; } = default!;
        public RowColumn DateOfIssue { get; set; } = default!;
        public RowColumn DueDate { get; set; } = default!;
        public RowColumn DateOfTaxableSupply { get; set; } = default!;
        public RowColumn InvoiceSubject { get; set; } = default!;
        public RowColumn PriceADay { get; set; } = default!;
        public RowColumn DaysCount { get; set; } = default!;
        public RowColumn ValueAddedTaxRate { get; set; } = default!;
        public RowColumn Price { get; set; } = default!;
        public RowColumn ValueAddedTax { get; set; } = default!;
        public RowColumn PriceWithValueAddedTax { get; set; } = default!;
        public RowColumn OverallPrice { get; set; } = default!;
        public RowColumn OverallValueAddedTaxRate { get; set; } = default!;
        public RowColumn OverallValueAddedTax { get; set; } = default!;
        public RowColumn OverallPriceWithValueAddedTax { get; set; } = default!;
    }
}
