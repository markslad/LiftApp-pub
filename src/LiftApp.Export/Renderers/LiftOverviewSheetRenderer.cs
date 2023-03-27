using LiftApp.Dal.Interfaces;
using LiftApp.Dal.Models;
using LiftApp.Export.Interfaces;
using LiftApp.Export.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Renderers
{
    public class LiftOverviewSheetRenderer : OverviewSheetRendererBase, IOverviewSheetRenderer
    {
        private readonly IOptions<LiftOverviewOptions> _liftOverviewOptions;

        public LiftOverviewSheetRenderer(IMainUnitOfWork mainUnitOfWork,
            IOptions<LiftOverviewOptions> liftOverviewOptions)
            : base(mainUnitOfWork)
        {
            _liftOverviewOptions = liftOverviewOptions;
        }

        public override async Task RenderContentAsync((DateOnly dateFrom, DateOnly dateTo)? dateRange = null)
        {
            var lifts = await _mainUnitOfWork.LiftRepository.GetAsync(include: lifts => lifts
            .Include(lift => lift.Office).ThenInclude(office => office.Address),
            orderBy: lifts => lifts
                .OrderBy(lift => lift.SerialNumber.Length)
                .ThenBy(lift => lift.SerialNumber));

            if (lifts is null)
                throw new NullReferenceException("Lifts collection recieved null");
            if (_worksheet is null)
                throw new NullReferenceException("Worksheet is null");

            _worksheet.Cells[_liftOverviewOptions.Value.ExportDateRowIndex, _liftOverviewOptions.Value.ExportDateColumnIndex] = DateTime.Now;

            if (dateRange is not null)
            {
                _worksheet.Cells[_liftOverviewOptions.Value.DateRangeRowIndex, _liftOverviewOptions.Value.DateRangeColumnIndex] = $"{dateRange.Value.dateFrom} - {dateRange.Value.dateTo}";
            }
            else
            {
                _worksheet.Cells[_liftOverviewOptions.Value.DateRangeRowIndex, _liftOverviewOptions.Value.DateRangeColumnIndex] = "-";
            }

            var startRow = _liftOverviewOptions.Value.StartRowIndex;
            foreach (var lift in lifts)
            {
                _worksheet.Cells[startRow, _liftOverviewOptions.Value.LiftSerialNumberColumnIndex] = lift.SerialNumber;
                _worksheet.Cells[startRow, _liftOverviewOptions.Value.LiftManufacturerColumnIndex] = lift.Manufacturer;
                _worksheet.Cells[startRow, _liftOverviewOptions.Value.LiftMaximumHeightColumnIndex] = lift.MaximumHeight;
                _worksheet.Cells[startRow, _liftOverviewOptions.Value.LiftPowerSourceColumnIndex] = lift.PowerSource.ToString();
                _worksheet.Cells[startRow, _liftOverviewOptions.Value.LiftEliminatedColumnIndex] = lift.Eliminated;
                _worksheet.Cells[startRow, _liftOverviewOptions.Value.OfficeAddressColumnIndex] = lift.Office.Address.Street;
                _worksheet.Cells[startRow, _liftOverviewOptions.Value.OfficeHouseNumberColumnIndex] = lift.Office.Address.HouseNumber;
                _worksheet.Cells[startRow, _liftOverviewOptions.Value.OfficeCityColumnIndex] = lift.Office.Address.City;
                _worksheet.Cells[startRow, _liftOverviewOptions.Value.OfficeZipCodeColumnIndex] = lift.Office.Address.ZipCode;
                _worksheet.Cells[startRow, _liftOverviewOptions.Value.OfficeCountryColumnIndex] = lift.Office.Address.Country;

                startRow++;

            }
            _worksheet.Columns.AutoFit();
        }
    }
}
