using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiftApp.Dal.Models;
using System;
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
    public partial class InvoicesViewModel : ObservableObject
    {
        [ObservableProperty]
        private ICommand? _showBorrowalDetailCommand;

        [ObservableProperty]
        private ObservableCollection<Invoice>? _invoices;

        partial void OnInvoicesChanged(ObservableCollection<Invoice>? value)
        {
            InvoicesView = CollectionViewSource.GetDefaultView(value);
        }

        [ObservableProperty]
        private ICollectionView? _invoicesView;

        [ObservableProperty]
        private string? _filter;

        partial void OnFilterChanged(string? value)
        {
            if (Invoices is null)
                throw new NullReferenceException("Invoicces items source is null");

            InvoicesView = CollectionViewSource.GetDefaultView(Invoices);
            if (value is null)
                return;

            InvoicesView.Filter = o =>
            {
                var i = (Invoice)o;
                return i.Identifier.Contains(value);
            };
        }

        [ObservableProperty]
        private Invoice? _selectedInvoice;

        partial void OnSelectedInvoiceChanged(Invoice? value)
        {
            if (value is not null)
            {
                ExportButtonEnabled = true;

                if (!value.Paid)
                {
                    ConfirmPaymentButtonEnabled = true;
                    DeleteButtonEnabled = true;
                }
                else
                {
                    ConfirmPaymentButtonEnabled = false;
                    DeleteButtonEnabled = false;

                }
            }
            else
            {
                ConfirmPaymentButtonEnabled = false;
                DeleteButtonEnabled = false;
                ExportButtonEnabled = false;
            }
        }

        [ObservableProperty]
        private bool _confirmPaymentButtonEnabled = false;

        [ObservableProperty]
        private bool _deleteButtonEnabled = false;

        [ObservableProperty]
        private bool _exportButtonEnabled = false;

        [ObservableProperty]
        private IAsyncRelayCommand? _showNewInvoiceCommand;

        [ObservableProperty]
        private IAsyncRelayCommand? _confirmPaymentCommand;

        [ObservableProperty]
        private IAsyncRelayCommand? _deleteInvoiceCommand;

        [ObservableProperty]
        private ICommand? _exportInvoiceCommand;
    }
}
