using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Options
{
    public class CustomerOverviewOptions
    {
        public int ExportDateColumnIndex { get; set; } = default!;
        public int ExportDateRowIndex { get; set; } = default!;
        public int DateRangeColumnIndex { get; set; } = default!;
        public int DateRangeRowIndex { get; set; } = default!;
        public int StartRowIndex { get; set; } = default!;
        public int IdentifierColumnIndex { get; set; } = default!;
        public int TaxIdentificationNumberColumnIndex { get; set; } = default!;
        public int PhoneNumberColumnIndex { get; set; } = default!;
        public int EmailColumnIndex { get; set; } = default!;
        public int FirstNameColumnIndex { get; set; } = default!;
        public int SurnameColumnIndex { get; set; } = default!;
        public int NameColumnIndex { get; set; } = default!;
        public int StreetColumnIndex { get; set; } = default!;
        public int HouseNumberColumnIndex { get; set; } = default!;
        public int CityColumnIndex { get; set; } = default!;
        public int ZipCodeColumnIndex { get; set; } = default!;
        public int CountryColumnIndex { get; set; } = default!;
    }
}   
