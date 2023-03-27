using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using LiftApp.Enums;

namespace LiftApp.Converters
{ 
    [ValueConversion(typeof(CustomerType), typeof(String))]
    public class CustomerTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return string.Empty;
            if (value is string)
                return string.Empty;
            var customerType = (CustomerType)value;
            switch (customerType)
            {
                case CustomerType.NonEntrepreneurCustomer:
                    return "Nepodnikatelská osoba";
                case CustomerType.OwnAccountWorker:
                    return "OSVČ";
                case CustomerType.LegalEntity:
                    return "Společnost";
                default:
                    throw new NotImplementedException($"Converter is not implemented for {value}");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
