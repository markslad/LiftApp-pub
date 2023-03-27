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
    [ValueConversion(typeof(Customer), typeof(String))]
    public class CustomerToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
                return string.Empty;
            if (value is null)
                return string.Empty;
            var customer = (Customer)value;
            if (customer is NonEntrepreneurCustomer)
                return $"Identifikátor: {customer.Identifier}, Jméno: {((NonEntrepreneurCustomer)customer).FirstName}, Příjmení: {((NonEntrepreneurCustomer)customer).Surname}, Tel. číslo: {customer.PhoneNumber}, Email: {customer.Email}";
            if (customer is OwnAccountWorker)
                return $"Identifikátor: {customer.Identifier}, DIČ: {((EntrepreneurCustomer)customer).TaxIdentificationNumber ?? "-"}, Jméno: {((OwnAccountWorker)customer).FirstName}, Příjmení: {((OwnAccountWorker)customer).Surname}, Tel. číslo: {customer.PhoneNumber}, Email: {customer.Email}";
            if (customer is LegalEntity)
                return $"Identifikátor: {customer.Identifier}, DIČ: {((EntrepreneurCustomer)customer).TaxIdentificationNumber ?? "-"}, Název: {((LegalEntity)customer).Name ?? "-"}, Tel. číslo: {customer.PhoneNumber}, Email: {customer.Email}";
            else
                throw new NotImplementedException($"{nameof(CustomerToStringConverter)} is not implemented for type: {customer.GetType()}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
}
