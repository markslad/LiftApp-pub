using LiftApp.Dal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Options
{
    public class MaintenanceOverviewOptions
    {
        public int ExportDateColumnIndex { get; set; } = default!;
        public int ExportDateRowIndex { get; set; } = default!;
        public int DateRangeColumnIndex { get; set; } = default!;
        public int DateRangeRowIndex { get; set; } = default!;
        public int StartRowIndex { get; set; } = default!;
        public int DescriptionColumnIndex { get; set; } = default!;
        public int PriceColumnIndex { get; set; } = default!;
        public int DateFromColumnIndex { get; set; } = default!;
        public int DateToColumnIndex { get; set; } = default!;
        public int SerialNumberColumnIndex { get; set; } = default!;
        public int ManufacturerColumnIndex { get; set; } = default!;
        public int MaximumHeightColumnIndex { get; set; } = default!;
        public int PowerSourceColumnIndex { get; set; } = default!;
        public int EliminatedColumnIndex { get; set; } = default!;
    }
}
