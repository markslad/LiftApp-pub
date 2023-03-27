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
    [ValueConversion(typeof(Lift), typeof(String))]
    public class LiftToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return default!;
            if(value is string)
            {
                if ((string)value == string.Empty)
                    return string.Empty;
            }

            var lift = (Lift)value;
            return $"Sériové číslo: {lift.SerialNumber}, Výrobce: {lift.Manufacturer}, Zdvih: {lift.MaximumHeight}, Pohon: {new PowerSourceToStringConverter().Convert(lift.PowerSource, targetType, parameter, culture)}, Vyřazena: {new BoolToStringConverter().Convert(lift.Eliminated, targetType, parameter, culture)}, Umístění: {new AddressToStringConverter().Convert(lift.Office.Address, targetType, parameter, culture)}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
}
