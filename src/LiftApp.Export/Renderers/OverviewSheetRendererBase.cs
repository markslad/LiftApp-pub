using LiftApp.Dal.Interfaces;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Renderers
{
    public abstract class OverviewSheetRendererBase
    {
        protected readonly IMainUnitOfWork _mainUnitOfWork;
        protected Worksheet? _worksheet;

        public OverviewSheetRendererBase(IMainUnitOfWork mainUnitOfWork)
        {
            _mainUnitOfWork = mainUnitOfWork;
        }

        public void InitializeWorksheet(Workbook? workbook, string worksheetName)
        {
            if (workbook is null)
                throw new NullReferenceException("Workbook is null");

            _worksheet = workbook.Worksheets[worksheetName] as Worksheet;
        }

        public abstract Task RenderContentAsync((DateOnly dateFrom, DateOnly dateTo)? dateRange = null);
    }
}
