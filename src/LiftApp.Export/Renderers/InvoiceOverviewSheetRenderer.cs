using LiftApp.Dal.Interfaces;
using LiftApp.Dal.Models;
using LiftApp.Export.Interfaces;
using LiftApp.Export.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Renderers
{
    public class InvoiceOverviewSheetRenderer : OverviewSheetRendererBase, IOverviewSheetRenderer
    {
        private readonly IOptions<InvoiceOverviewOptions> _invoiceOverviewOptions;

        public InvoiceOverviewSheetRenderer(IOptions<InvoiceOverviewOptions> invoiceOverviewOptions,
            IMainUnitOfWork mainUnitOfWork)
            : base(mainUnitOfWork)
        {
            _invoiceOverviewOptions = invoiceOverviewOptions;
        }

        public override async Task RenderContentAsync((DateOnly dateFrom, DateOnly dateTo)? dateRange = null)
        {
            IEnumerable<Invoice>? invoices;

            if (dateRange is not null)
            {
                _worksheet!.Cells[_invoiceOverviewOptions.Value.DateRangeRowIndex, _invoiceOverviewOptions.Value.DateRangeColumnIndex] = $"{dateRange.Value.dateFrom} - {dateRange.Value.dateTo}";
                invoices = await _mainUnitOfWork.InvoiceRepository.GetAsync(include: invoices => invoices
                    .Include(invoice => invoice.Borrowal).ThenInclude(borrowal => borrowal.TimeInterval),
                filter: invoice =>
                    invoice.DateOfIssue >= dateRange.Value.dateFrom
                    && invoice.DateOfIssue <= dateRange.Value.dateTo,
                orderBy: invoices => invoices
                    .OrderBy(invoice => invoice.Identifier.Length)
                    .ThenBy(invoice => invoice.Identifier));
            }
            else
            {
                _worksheet!.Cells[_invoiceOverviewOptions.Value.DateRangeRowIndex, _invoiceOverviewOptions.Value.DateRangeColumnIndex] = "-";
                invoices = await _mainUnitOfWork.InvoiceRepository.GetAsync(include: invoices => invoices
                    .Include(invoice => invoice.Borrowal).ThenInclude(borrowal => borrowal.TimeInterval),
                    orderBy: invoices => invoices
                    .OrderBy(invoice => invoice.Identifier.Length)
                    .ThenBy(invoice => invoice.Identifier));
            }

            if (invoices is null)
                throw new NullReferenceException("Invoices collection recieved null");
            if (_worksheet is null)
                throw new NullReferenceException("Worksheet is null");

            _worksheet.Cells[_invoiceOverviewOptions.Value.ExportDateRowIndex, _invoiceOverviewOptions.Value.ExportDateColumnIndex] = DateTime.Now;

            var startRow = _invoiceOverviewOptions.Value.StartRowIndex;
            foreach (var invoice in invoices)
            {
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.InvoiceIdentifierColumnIndex] = invoice.Identifier;
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.DateOfIssueColumnIndex] = invoice.DateOfIssue.ToString();
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.DueDateColumnIndex] = invoice.DueDate.ToString();
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.DateOfTaxableSupplyColumnIndex] = invoice.DateOfTaxableSupply.ToString();
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.PriceColumnIndex] = invoice.Price;
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.ValueAddedTaxRateColumnIndex] = invoice.ValueAddedTaxRate;
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.PriceWithValueAddedTaxColumnIndex] = invoice.Price * (1 + invoice.ValueAddedTaxRate);
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.BankColumnIndex] = invoice.Bank;
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.BankAccountColumnIndex] = invoice.BankAccount;
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.VariableSymbolColumnIndex] = invoice.VariableSymbol;
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.IsExtraColumnIndex] = invoice.IsExtra;
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.PaidColumnIndex] = invoice.Paid;
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.BorrowalIDColumnIndex] = invoice.Borrowal.Id;
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.BorrowalDateFromColumnIndex] = invoice.Borrowal.TimeInterval.DateFrom.ToString();
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.BorrowalDateToColumnIndex] = invoice.Borrowal.TimeInterval.DateTo.ToString();
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.BorrowalPriceADayColumnIndex] = invoice.Borrowal.PriceADay;
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.LiftIdentifierColumnIndex] = invoice.Borrowal.LiftSerialNumber;
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.CustomerIdentifierColumnIndex] = invoice.Borrowal.CustomerIdentifier;
                _worksheet.Cells[startRow, _invoiceOverviewOptions.Value.DescriptionColumnIndex] = invoice.Description;

                startRow++;

            }
            _worksheet.Columns.AutoFit();
        }
    }
}
