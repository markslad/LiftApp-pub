using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiftApp.Dal.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace LiftApp.ViewModels
{
    public partial class BorrowalsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ICommand? _showLiftDetailCommand;

        [ObservableProperty]
        private ICommand? _showCustomerDetailCommand;

        [ObservableProperty]
        private IAsyncRelayCommand? _showNewBorrowalCommand;

        [ObservableProperty]
        private IAsyncRelayCommand? _deleteBorrowalCommand;

        [ObservableProperty]
        private IAsyncRelayCommand? _showNewInvoiceCommand;

        [ObservableProperty]
        private ObservableCollection<Borrowal>? _borrowals;

        partial void OnBorrowalsChanged(ObservableCollection<Borrowal>? value)
        {
            BorrowalsView = CollectionViewSource.GetDefaultView(value);
        }

        [ObservableProperty]
        private ICollectionView? _borrowalsView;

        [ObservableProperty]
        private Borrowal? _selectedBorrowal;

        partial void OnSelectedBorrowalChanged(Borrowal? value)
        {
            if (value is not null)
            {
                if (DateTime.Now < value.TimeInterval.DateTo.ToDateTime(TimeOnly.Parse("00:00")))
                    DeleteButtonEnabled = true;
                else DeleteButtonEnabled = false;

                NewInvoiceButtonEnabled = true;
            }
            else
            {
                DeleteButtonEnabled = false;
                NewInvoiceButtonEnabled = false;
            }
        }

        [ObservableProperty]
        private string? _filter;

        partial void OnFilterChanged(string? value)
        {
            if (Borrowals is null)
                throw new NullReferenceException("Borrowals items source is null");

            BorrowalsView = CollectionViewSource.GetDefaultView(Borrowals);
            if (value is null)
                return;

            BorrowalsView.Filter = o =>
            {
                var b = (Borrowal)o;
                return b.Customer.Identifier.Contains(value)
                || b.Lift.SerialNumber.Contains(value);
            };
        }

        [ObservableProperty]   
        private bool _deleteButtonEnabled = false;

        [ObservableProperty]
        private bool _newInvoiceButtonEnabled = false;
    }
}
      