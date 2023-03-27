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
    public class MaintenanceOverviewSheetRenderer : OverviewSheetRendererBase, IOverviewSheetRenderer
    {
        private readonly IOptions<MaintenanceOverviewOptions> _maintenanceOverviewOptions;

        public MaintenanceOverviewSheetRenderer(IOptions<MaintenanceOverviewOptions> maintenanceOverviewOptions,
        IMainUnitOfWork mainUnitOfWork)
            : base(mainUnitOfWork)
        {
            _maintenanceOverviewOptions = maintenanceOverviewOptions;
        }

        public override async Task RenderContentAsync((DateOnly dateFrom, DateOnly dateTo)? dateRange = null)
        {
            IEnumerable<Maintenance>? maintenances;

            if (dateRange is not null)
            {
                _worksheet!.Cells[_maintenanceOverviewOptions.Value.DateRangeRowIndex, _maintenanceOverviewOptions.Value.DateRangeColumnIndex] = $"{dateRange.Value.dateFrom} - {dateRange.Value.dateTo}";
                maintenances = await _mainUnitOfWork.MaintenanceRepository.GetAsync(include: maintenances => maintenances
                    .Include(maintenance => maintenance.TimeInterval)
                    .Include(maintenance => maintenance.Lift),
                filter: maintenance =>
                    maintenance.TimeInterval.DateFrom >= dateRange.Value.dateFrom
                    && maintenance.TimeInterval.DateTo <= dateRange.Value.dateTo,
                orderBy: maintnenances => maintnenances
                    .OrderBy(maintenance => maintenance.TimeInterval.DateFrom));
            }
            else
            {
                _worksheet!.Cells[_maintenanceOverviewOptions.Value.DateRangeRowIndex, _maintenanceOverviewOptions.Value.DateRangeColumnIndex] = "-";
                maintenances = await _mainUnitOfWork.MaintenanceRepository.GetAsync(include: maintenances => maintenances
                    .Include(maintenance => maintenance.TimeInterval)
                    .Include(maintenance => maintenance.Lift),
                orderBy: maintnenances => maintnenances
                    .OrderBy(maintenance => maintenance.TimeInterval.DateFrom));
            }

            if (maintenances is null)
                throw new NullReferenceException("Maintenances collection recieved null");
            if (_worksheet is null)
                throw new NullReferenceException("Worksheet is null");

            _worksheet.Cells[_maintenanceOverviewOptions.Value.ExportDateRowIndex, _maintenanceOverviewOptions.Value.ExportDateColumnIndex] = DateTime.Now;

            var startRow = _maintenanceOverviewOptions.Value.StartRowIndex;
            foreach (var maintenance in maintenances)
            {
                _worksheet.Cells[startRow, _maintenanceOverviewOptions.Value.DateFromColumnIndex] = maintenance.TimeInterval.DateFrom.ToString();
                _worksheet.Cells[startRow, _maintenanceOverviewOptions.Value.DateToColumnIndex] = maintenance.TimeInterval.DateTo.ToString();
                _worksheet.Cells[startRow, _maintenanceOverviewOptions.Value.DescriptionColumnIndex] = maintenance.Description;
                _worksheet.Cells[startRow, _maintenanceOverviewOptions.Value.PriceColumnIndex] = maintenance.Price;
                _worksheet.Cells[startRow, _maintenanceOverviewOptions.Value.SerialNumberColumnIndex] = maintenance.LiftSerialNumber;
                _worksheet.Cells[startRow, _maintenanceOverviewOptions.Value.ManufacturerColumnIndex] = maintenance.Lift.Manufacturer;
                _worksheet.Cells[startRow, _maintenanceOverviewOptions.Value.MaximumHeightColumnIndex] = maintenance.Lift.MaximumHeight;
                _worksheet.Cells[startRow, _maintenanceOverviewOptions.Value.PowerSourceColumnIndex] = maintenance.Lift.PowerSource.ToString();
                _worksheet.Cells[startRow, _maintenanceOverviewOptions.Value.EliminatedColumnIndex] = maintenance.Lift.Eliminated;

                startRow++;

            }
            _worksheet.Columns.AutoFit();
        }
    }
}
