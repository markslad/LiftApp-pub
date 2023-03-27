using LiftApp.Dal.Interfaces;
using LiftApp.Export.Enums;
using LiftApp.Export.Factories;
using LiftApp.Export.Interfaces;
using LiftApp.Export.Models;
using LiftApp.Export.Options;
using LiftApp.Export.Renderers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Services
{
    public class OverviewExportService : ExportServiceBase
    {
        private readonly IOptions<OverviewExportOptions> _overviewExportOptions;

        public OverviewExportService(
            IOptions<OverviewExportOptions> overviewExportOptions,
            ILogger<OverviewExportService> logger,
            IServiceProvider serviceProvider) :
            base(serviceProvider, logger)
        {
            _overviewExportOptions = overviewExportOptions;
        }

        public async Task ExportAsync(string directoryPath, IEnumerable<OverviewConfiguration> overviewConfigurations)
        {
            try
            {
                InitializeExcelApp();
                InitializeWorkbook(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, _overviewExportOptions.Value.ExcelTemplatePath));

                // Render contents
                foreach (var overviewConfiguration in overviewConfigurations)
                {
                    var renderer = _serviceProvider.GetRequiredService<OverviewSheetRendererFactory>().Create(overviewConfiguration.OverviewType);
                    renderer.InitializeWorksheet(_workbook, _overviewExportOptions.Value.SheetNames[overviewConfiguration.OverviewType]);
                    await renderer.RenderContentAsync(overviewConfiguration.DateRange);
                }

                foreach(var sheetName in _overviewExportOptions.Value.SheetNames)
                {
                    if (!overviewConfigurations.Select(oc => oc.OverviewType).Contains(sheetName.Key))
                    {
                        _workbook!.Worksheets[sheetName.Value].Delete();
                    }
                }


                // Save
                var fileName = Path.Combine(directoryPath, $"Export - {DateTime.Now:dd-MM-yyyy HH-mm}.xlsx");
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

            _workbook.SaveCopyAs(fileName);
            _logger.LogInformation($"Saved file as: {fileName}");
        }
    }
}
