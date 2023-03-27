using Microsoft.Extensions.Logging;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Services
{
    public abstract class ExportServiceBase
    {
        protected readonly IServiceProvider _serviceProvider;
        protected Application? _excelApp;
        protected Workbook? _workbook;
        protected readonly ILogger<ExportServiceBase> _logger;

        public ExportServiceBase(IServiceProvider serviceProvider,
            ILogger<ExportServiceBase> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected void InitializeExcelApp()
        {
            _excelApp = new Application();
            _excelApp.DisplayAlerts = false;
            _logger.LogInformation("Opened Excel application");
        }

        protected void InitializeWorkbook(string tempplatePath)
        {
            if (_excelApp is null)
                throw new NullReferenceException("Excel app is null");

            var templatePath = tempplatePath;
            _workbook = _excelApp?.Workbooks.Open(templatePath);
            _logger.LogInformation("Opened Excel workbook");
        }

        protected void CloseExcelApp()
        {
            if (_excelApp is null)
                throw new NullReferenceException("Excel app is null");

            _excelApp.Quit();
            _logger.LogInformation("Closed Excel application");
        }

        protected void CloseWorkbook()
        {
            _workbook?.Close(0);
            _logger.LogInformation("Closed Excel workbook");
        }

        protected abstract void SaveAs(string fileName);
    }
}
