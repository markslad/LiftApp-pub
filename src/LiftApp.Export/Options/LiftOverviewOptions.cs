using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Options
{
    public class LiftOverviewOptions
    {
        public int ExportDateColumnIndex { get; set; } = default!;
        public int ExportDateRowIndex { get; set; } = default!;
        public int DateRangeColumnIndex { get; set; } = default!;
        public int DateRangeRowIndex { get; set; } = default!;
        public int StartRowIndex { get; set; } = default!;
        public int LiftSerialNumberColumnIndex { get; set; } = default!;
        public int LiftManufacturerColumnIndex { get; set; } = default!;
        public int LiftMaximumHeightColumnIndex { get; set; } = default!;
        public int LiftPowerSourceColumnIndex { get; set; } = default!;
        public int LiftEliminatedColumnIndex { get; set; } = default!;
        public int OfficeAddressColumnIndex { get; set; } = default!;
        public int OfficeHouseNumberColumnIndex { get; set; } = default!;
        public int OfficeCityColumnIndex { get; set; } = default!;
        public int OfficeZipCodeColumnIndex { get; set; } = default!;
        public int OfficeCountryColumnIndex { get; set; } = default!;
    }
}
