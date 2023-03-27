using LiftApp.Statistics.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LiftApp.Converters
{
    [ValueConversion(typeof(BorrowalReportType), typeof(String))]
    public class BorrowalReportTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
                return string.Empty;

            var borrowalReportType = (BorrowalReportType)value;
            return borrowalReportType switch
            {
                BorrowalReportType.OverallIncomeByDays => "Souhrnné tržby podle dní",
                BorrowalReportType.IncomeByCustomers => "Tržby ve dnech podle zákazníků",
                BorrowalReportType.BorrowedLiftTypeByDays => "Počet vypůjčených kusů plošin ve dnech podle typu",
                BorrowalReportType.IncomeYearComparison => "Srovnání tržeb v měsících vybraných let",
                _ => throw new NotImplementedException("Unkonwn Income report type")
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
