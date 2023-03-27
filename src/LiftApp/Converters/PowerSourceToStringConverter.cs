using LiftApp.Dal.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LiftApp.Converters
{
    [ValueConversion(typeof(PowerSource), typeof(String))]
    public class PowerSourceToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
                return string.Empty;

            PowerSource powerSource = (PowerSource)value;
            switch (powerSource)
            {
                case PowerSource.Diesel:
                    return "Diesel";
                case PowerSource.Electric:
                    return "Elektrický";
                default:
                    throw new NotImplementedException($"Converter not implemented for value: {powerSource}");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
