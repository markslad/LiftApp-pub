using LiftApp.Dal.Models;
using LiftApp.Export.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Services
{
    public class InvoiceExportService : ExportServiceBase
    {
        private readonly IOptions<InvoiceExportOptions> _invoiceExportOptions;
        private readonly IOptions<CompanyOptions> _companyOptions;
        private Worksheet? _worksheet;

        public InvoiceExportService(
            IOptions<InvoiceExportOptions> invoiceExportOptions,
            IOptions<CompanyOptions> companyOptions,
            ILogger<InvoiceExportService> logger,
            IServiceProvider serviceProvider) :
            base(serviceProvider, logger)
        {
            _invoiceExportOptions = invoiceExportOptions;
            _companyOptions = companyOptions;
        }

        public void Export(string directoryPath, Invoice invoice)
        {
            try
            {
                InitializeExcelApp();
                InitializeWorkbook(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, _invoiceExportOptions.Value.ExcelTemplatePath));

                // Render contents
                // Contractor
                _worksheet = _workbook!.Worksheets[1];
                _worksheet.Cells[_invoiceExportOptions.Value.ContractorName.RowIndex, _invoiceExportOptions.Value.ContractorName.ColumnIndex].Value = _companyOptions.Value.Name;
                _worksheet.Cells[_invoiceExportOptions.Value.ContractorAddress.RowIndex, _invoiceExportOptions.Value.ContractorAddress.ColumnIndex].Value = _companyOptions.Value.Address;
                _worksheet.Cells[_invoiceExportOptions.Value.ContractorIdentifier.RowIndex, _invoiceExportOptions.Value.ContractorIdentifier.ColumnIndex].Value = _companyOptions.Value.Identifier;
                _worksheet.Cells[_invoiceExportOptions.Value.ContractorTaxIdentificationNumber.RowIndex, _invoiceExportOptions.Value.ContractorTaxIdentificationNumber.ColumnIndex].Value = _companyOptions.Value.TaxIdentificationNumber;
                // Customer
                _worksheet.Cells[_invoiceExportOptions.Value.CustomerIdentifier.RowIndex, _invoiceExportOptions.Value.CustomerIdentifier.ColumnIndex].Value = invoice.Borrowal.CustomerIdentifier;
                var customerAddress = invoice.Borrowal.Customer.Address;
                _worksheet.Cells[_invoiceExportOptions.Value.CustomerAddress.RowIndex, _invoiceExportOptions.Value.CustomerAddress.ColumnIndex].Value =
                    $"{customerAddress.Street} {customerAddress.HouseNumber}, {customerAddress.City}, {customerAddress.ZipCode} {customerAddress.Country}";
                switch (invoice.Borrowal.Customer)
                {
                    case NonEntrepreneurCustomer customer:
                        _worksheet.Cells[_invoiceExportOptions.Value.CustomerName.RowIndex, _invoiceExportOptions.Value.CustomerName.ColumnIndex] = $"{customer.FirstName} {customer.Surname}";
                        break;
                    case OwnAccountWorker customer:
                        _worksheet.Cells[_invoiceExportOptions.Value.CustomerName.RowIndex, _invoiceExportOptions.Value.CustomerName.ColumnIndex] = $"{customer.FirstName} {customer.Surname}";
                        _worksheet.Cells[_invoiceExportOptions.Value.CustomerTaxIdentificationNumber.RowIndex, _invoiceExportOptions.Value.CustomerTaxIdentificationNumber.ColumnIndex] = customer.TaxIdentificationNumber;
                        break;
                    case LegalEntity customer:
                        _worksheet.Cells[_invoiceExportOptions.Value.CustomerName.RowIndex, _invoiceExportOptions.Value.CustomerName.ColumnIndex] = customer.Name;
                        _worksheet.Cells[_invoiceExportOptions.Value.CustomerTaxIdentificationNumber.RowIndex, _invoiceExportOptions.Value.CustomerTaxIdentificationNumber.ColumnIndex] = customer.TaxIdentificationNumber;
                        break;
                    default:
                        throw new NotImplementedException("Unknown customer type");
                }
                // Invoice content
                _worksheet.Cells[_invoiceExportOptions.Value.InvoiceIdentifier.RowIndex, _invoiceExportOptions.Value.InvoiceIdentifier.ColumnIndex].Value = invoice.Identifier;
                _worksheet.Cells[_invoiceExportOptions.Value.ContractorBank.RowIndex, _invoiceExportOptions.Value.ContractorBank.ColumnIndex].Value = invoice.Bank;
                _worksheet.Cells[_invoiceExportOptions.Value.ContractorBankAccount.RowIndex, _invoiceExportOptions.Value.ContractorBankAccount.ColumnIndex].Value = invoice.BankAccount;
                _worksheet.Cells[_invoiceExportOptions.Value.ContractorVariableSymbol.RowIndex, _invoiceExportOptions.Value.ContractorVariableSymbol.ColumnIndex].Value = invoice.VariableSymbol;
                _worksheet.Cells[_invoiceExportOptions.Value.DateOfIssue.RowIndex, _invoiceExportOptions.Value.DateOfIssue.ColumnIndex].Value = invoice.DateOfIssue.ToString();
                _worksheet.Cells[_invoiceExportOptions.Value.DueDate.RowIndex, _invoiceExportOptions.Value.DueDate.ColumnIndex].Value = invoice.DueDate.ToString();
                _worksheet.Cells[_invoiceExportOptions.Value.DateOfTaxableSupply.RowIndex, _invoiceExportOptions.Value.DateOfTaxableSupply.ColumnIndex].Value = invoice.DateOfTaxableSupply.ToString();

                if(invoice.Description is null)
                {
                    var invoiceSubject = $"Plošina: {invoice.Borrowal.LiftSerialNumber}, Datum výpůjčky: {invoice.Borrowal.TimeInterval.DateFrom} - {invoice.Borrowal.TimeInterval.DateTo}";
                    _worksheet.Cells[_invoiceExportOptions.Value.InvoiceSubject.RowIndex, _invoiceExportOptions.Value.InvoiceSubject.ColumnIndex].Value = invoiceSubject;
                }
                else
                {
                    var invoiceSubject = $"Plošina: {invoice.Borrowal.LiftSerialNumber}, Datum výpůjčky: {invoice.Borrowal.TimeInterval.DateFrom} - {invoice.Borrowal.TimeInterval.DateTo}, Popis: {invoice.Description}";
                    _worksheet.Cells[_invoiceExportOptions.Value.InvoiceSubject.RowIndex, _invoiceExportOptions.Value.InvoiceSubject.ColumnIndex].Value = invoiceSubject;
                }

                _worksheet.Cells[_invoiceExportOptions.Value.PriceADay.RowIndex, _invoiceExportOptions.Value.PriceADay.ColumnIndex].Value = invoice.Borrowal.PriceADay;
                _worksheet.Cells[_invoiceExportOptions.Value.DaysCount.RowIndex, _invoiceExportOptions.Value.DaysCount.ColumnIndex].Value =
                    invoice.Borrowal.TimeInterval.DateTo.DayNumber - invoice.Borrowal.TimeInterval.DateFrom.DayNumber + 1;
                _worksheet.Cells[_invoiceExportOptions.Value.ValueAddedTaxRate.RowIndex, _invoiceExportOptions.Value.ValueAddedTaxRate.ColumnIndex].Value = invoice.ValueAddedTaxRate;
                _worksheet.Cells[_invoiceExportOptions.Value.Price.RowIndex, _invoiceExportOptions.Value.Price.ColumnIndex].Value = invoice.Price;
                _worksheet.Cells[_invoiceExportOptions.Value.ValueAddedTax.RowIndex, _invoiceExportOptions.Value.ValueAddedTax.ColumnIndex].Value = invoice.Price * invoice.ValueAddedTaxRate;
                _worksheet.Cells[_invoiceExportOptions.Value.PriceWithValueAddedTax.RowIndex, _invoiceExportOptions.Value.PriceWithValueAddedTax.ColumnIndex].Value = invoice.Price * (1 + invoice.ValueAddedTaxRate);
                _worksheet.Cells[_invoiceExportOptions.Value.OverallPrice.RowIndex, _invoiceExportOptions.Value.OverallPrice.ColumnIndex].Value = invoice.Price;
                _worksheet.Cells[_invoiceExportOptions.Value.OverallValueAddedTaxRate.RowIndex, _invoiceExportOptions.Value.OverallValueAddedTaxRate.ColumnIndex].Value = invoice.ValueAddedTaxRate;
                _worksheet.Cells[_invoiceExportOptions.Value.OverallValueAddedTax.RowIndex, _invoiceExportOptions.Value.OverallValueAddedTax.ColumnIndex].Value = invoice.Price * invoice.ValueAddedTaxRate;
                _worksheet.Cells[_invoiceExportOptions.Value.OverallPriceWithValueAddedTax.RowIndex, _invoiceExportOptions.Value.OverallPriceWithValueAddedTax.ColumnIndex].Value = invoice.Price * (1 + invoice.ValueAddedTaxRate);

                // Save
                var fileName = Path.Combine(directoryPath, $"{invoice.Identifier}_{DateTime.Now:dd-MM-yyyy_HH-mm}.pdf");
                SaveAs(fileName);
            }
            finally
            {
                CloseWorkbook();
                CloseExcelApp();
            }
        }

        protected override void SaveAs(string fileName)
        {
            if (_workbook is null)
                throw new NullReferenceException("Workbook is null");

            _workbook!.ExportAsFixedFormat(
                    XlFixedFormatType.xlTypePDF,
                    fileName,
                    XlFixedFormatQuality.xlQualityStandard,
                    false,
                    true,
                    OpenAfterPublish: false);
            _logger.LogInformation($"Saved file as: {fileName}");
        }
    }
}
