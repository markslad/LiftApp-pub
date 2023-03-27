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
    [ValueConversion(typeof(Borrowal), typeof(String))]
    public class BorrowalToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return string.Empty;
            if (value is string)
                return string.Empty;

            var borrowal = (Borrowal)value;
            return $"ID: {borrowal.Id}, " +
                $"Zákazník: {borrowal.CustomerIdentifier}, " +
                $"Plošina: {borrowal.LiftSerialNumber}, " +
                $"Datum od: {new DateToStringConverter().Convert(borrowal.TimeInterval.DateFrom, targetType, parameter, culture)}, " +
                $"Datum do: {new DateToStringConverter().Convert(borrowal.TimeInterval.DateTo, targetType, parameter, culture)}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
}
