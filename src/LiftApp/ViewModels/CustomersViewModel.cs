using CommunityToolkit.Mvvm.ComponentModel;
using LiftApp.Dal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace LiftApp.ViewModels
{
    public partial class CustomersViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Customer>? _customers;

        partial void OnCustomersChanged(ObservableCollection<Customer>? value)
        {
            CustomersView = CollectionViewSource.GetDefaultView(value);
        }

        [ObservableProperty]
        private ICollectionView? _customersView;

        [ObservableProperty]
        private string? _filter;

        partial void OnFilterChanged(string? value)
        {
            if (Customers is null)
                throw new NullReferenceException("Customers items source is null");

            CustomersView = CollectionViewSource.GetDefaultView(Customers);
            if (value is null)
                return;

            CustomersView.Filter = o =>
            {
                var c = (Customer)o;
                return c.Identifier.Contains(value)
                || c.PhoneNumber.Contains(value)
                || c.Email.Contains(value);
            };
        }
    }
}
