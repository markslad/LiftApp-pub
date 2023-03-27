using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Options
{
    public class BorrowalOverviewOptions
    {
        public int ExportDateColumnIndex { get; set; } = default!;
        public int ExportDateRowIndex { get; set; } = default!;
        public int DateRangeColumnIndex { get; set; } = default!;
        public int DateRangeRowIndex { get; set; } = default!;
        public int StartRowIndex { get; set; } = default!;
        public int BorrowalIdColumnIndex { get; set; } = default!;
        public int BorrowalDateFromColumnIndex { get; set; } = default!;
        public int BorrowalDateToColumnIndex { get; set; } = default!;
        public int BorrowalPriceADayColumnIndex { get; set; } = default!;
        public int LiftSerialNumberColumnIndex { get; set; } = default!;
        public int LiftManufacturerColumnIndex { get; set; } = default!;
        public int LiftMaximumHeightColumnIndex { get; set; } = default!;
        public int LiftPowerSourceColumnIndex { get; set; } = default!;
        public int LiftEliminatedColumnIndex { get; set; } = default!;
        public int CustomerIdentifierColumnIndex { get; set; } = default!;
        public int CustomerTaxIdentificationNumberColumnIndex { get; set; } = default!;
        public int CustomerPhoneNumberColumnIndex { get; set; } = default!;
        public int CustomerEmailColumnIndex { get; set; } = default!;
        public int CustomerFirstNameColumnIndex { get; set; } = default!;
        public int CustomerSurnameColumnIndex { get; set; } = default!;
        public int CustomerNameColumnIndex { get; set; } = default!;
        public int InvoicesColumnIndex { get; set; } = default!;

    }
}
