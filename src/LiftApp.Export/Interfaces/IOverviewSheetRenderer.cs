using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Interfaces
{
    public interface IOverviewSheetRenderer
    {
        void InitializeWorksheet(Workbook? workbook, string worksheetName);
        Task RenderContentAsync((DateOnly dateFrom, DateOnly dateTo)? dateRange = null);
    }
}
