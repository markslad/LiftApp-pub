using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Office.Interop.Excel;
using LiftApp.Export.Options;
using LiftApp.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;
using LiftApp.Dal.Models;
using LiftApp.Export.Interfaces;

namespace LiftApp.Export.Renderers
{
    public class BorrowalOverviewSheetRenderer : OverviewSheetRendererBase, IOverviewSheetRenderer
    {
        private readonly IOptions<BorrowalOverviewOptions> _borrowalOverviewOptions;

        public BorrowalOverviewSheetRenderer(IOptions<BorrowalOverviewOptions> borrowalOverviewOptions,
            IMainUnitOfWork mainUnitOfWork)
            : base(mainUnitOfWork)
        {
            _borrowalOverviewOptions = borrowalOverviewOptions;
        }

        public override async Task RenderContentAsync((DateOnly dateFrom, DateOnly dateTo)? dateRange = null)
        {
            IEnumerable<Borrowal>? borrowals;

            if (dateRange is not null)
            {
                _worksheet!.Cells[_borrowalOverviewOptions.Value.DateRangeRowIndex, _borrowalOverviewOptions.Value.DateRangeColumnIndex] = $"{dateRange.Value.dateFrom} - {dateRange.Value.dateTo}";
                borrowals = await _mainUnitOfWork.BorrowalRepository.GetAsync(include: borrowals => borrowals
                    .Include(borrowal => borrowal.TimeInterval)
                    .Include(borrowal => borrowal.Lift)
                    .Include(borrowal => borrowal.Customer),
                filter: borrowal => 
                    borrowal.TimeInterval.DateFrom >= dateRange.Value.dateFrom
                    && borrowal.TimeInterval.DateFrom <= dateRange.Value.dateTo,
                orderBy: borrowals => borrowals.OrderBy(borrowal => borrowal.TimeInterval.DateFrom));
            }
            else
            {
                _worksheet!.Cells[_borrowalOverviewOptions.Value.DateRangeRowIndex, _borrowalOverviewOptions.Value.DateRangeColumnIndex] = "-";
                borrowals = await _mainUnitOfWork.BorrowalRepository.GetAsync(include: borrowals => borrowals
                    .Include(borrowal => borrowal.TimeInterval)
                    .Include(borrowal => borrowal.Lift)
                    .Include(borrowal => borrowal.Customer)
                    .Include(borrowal => borrowal.Invoices),
                    orderBy: borrowals => borrowals.OrderBy(borrowal => borrowal.TimeInterval.DateFrom));
            }

            

            if (borrowals is null)
                throw new NullReferenceException("Borrowals collection recieved null");
            if (_worksheet is null)
                throw new NullReferenceException("Worksheet is null");

            _worksheet.Cells[_borrowalOverviewOptions.Value.ExportDateRowIndex, _borrowalOverviewOptions.Value.ExportDateColumnIndex] = DateTime.Now;

            

            var startRow = _borrowalOverviewOptions.Value.StartRowIndex;
            foreach (var borrowal in borrowals)
            {
                _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.BorrowalIdColumnIndex] = borrowal.Id;
                _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.BorrowalDateFromColumnIndex] = borrowal.TimeInterval.DateFrom.ToString();
                _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.BorrowalDateToColumnIndex] = borrowal.TimeInterval.DateTo.ToString();
                _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.BorrowalPriceADayColumnIndex] = borrowal.PriceADay;
                _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.LiftSerialNumberColumnIndex] = borrowal.LiftSerialNumber;
                _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.LiftManufacturerColumnIndex] = borrowal.Lift.Manufacturer;
                _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.LiftMaximumHeightColumnIndex] = borrowal.Lift.MaximumHeight;
                _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.LiftPowerSourceColumnIndex] = borrowal.Lift.PowerSource.ToString();
                _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.LiftEliminatedColumnIndex] = borrowal.Lift.Eliminated;
                _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.CustomerIdentifierColumnIndex] = borrowal.Customer.Identifier;
                _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.CustomerPhoneNumberColumnIndex] = borrowal.Customer.PhoneNumber;
                _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.CustomerEmailColumnIndex] = borrowal.Customer.Email;

                switch (borrowal.Customer)
                {
                    case NonEntrepreneurCustomer customer:
                        _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.CustomerFirstNameColumnIndex] = customer.FirstName;
                        _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.CustomerSurnameColumnIndex] = customer.Surname;
                        break;
                    case OwnAccountWorker customer:
                        _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.CustomerTaxIdentificationNumberColumnIndex] = customer.TaxIdentificationNumber;
                        _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.CustomerFirstNameColumnIndex] = customer.FirstName;
                        _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.CustomerSurnameColumnIndex] = customer.Surname;
                        break;
                    case LegalEntity customer:
                        _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.CustomerTaxIdentificationNumberColumnIndex] = customer.TaxIdentificationNumber;
                        _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.CustomerNameColumnIndex] = customer.Name;
                        break;
                    default:
                        throw new NotImplementedException("Unkown customer type");
                }

                _worksheet.Cells[startRow, _borrowalOverviewOptions.Value.InvoicesColumnIndex] = string.Join(';', borrowal.Invoices.Select(invoice => invoice.Identifier));

                startRow++;

            }
            _worksheet.Columns.AutoFit();
        }
    }
}
