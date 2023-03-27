using LiftApp.Dal.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LiftApp.Converters
{
    [ValueConversion(typeof(IEnumerable<Invoice>), typeof(String))]
    public class InvoicesToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var invoices = (IEnumerable<Invoice>)value;
            return string.Join(';', invoices.Select(invoice => invoice.Identifier));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
}
