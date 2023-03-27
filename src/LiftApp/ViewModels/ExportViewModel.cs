using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftApp.ValidationAttributes;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace LiftApp.ViewModels
{
    public partial class ExportViewModel : ObservableValidator
    {
        [ObservableProperty]
        private bool _borrowalChecked = false;
        partial void OnBorrowalCheckedChanged(bool value)
        {
            BorrowalDateFrom = null;
            BorrowalDateTo = null;
            if (value == true)
                BorrowalDateRangeEnabled = true;
            else
                BorrowalDateRangeEnabled = false;

            ToggleSubmitButtonEnabled();
        }

        [ObservableProperty]
        private bool _liftChecked = false;
        partial void OnLiftCheckedChanged(bool value)
        {
            ToggleSubmitButtonEnabled();
        }

        [ObservableProperty]
        private bool _maintenanceChecked = false;
        partial void OnMaintenanceCheckedChanged(bool value)
        {
            MaintenanceDateFrom = null;
            MaintenanceDateTo = null;
            if (value == true)
                MaintenanceDateRangeEnabled = true;
            else
                MaintenanceDateRangeEnabled = false;

            ToggleSubmitButtonEnabled();
        }

        [ObservableProperty]
        private bool _invoiceChecked = false;
        partial void OnInvoiceCheckedChanged(bool value)
        {
            InvoiceDateFrom = null;
            InvoiceDateTo = null;
            if (value == true)
                InvoiceDateRangeEnabled = true;
            else
                InvoiceDateRangeEnabled = false;

            ToggleSubmitButtonEnabled();
        }

        [ObservableProperty]
        private bool _customerChecked = false;
        partial void OnCustomerCheckedChanged(bool value)
        {
            ToggleSubmitButtonEnabled();
        }

        [ObservableProperty]
        [SameReference(nameof(BorrowalDateTo))]
        [EarlierThan(nameof(BorrowalDateTo))]
        private DateTime? _borrowalDateFrom;

        [ObservableProperty]
        [SameReference(nameof(BorrowalDateFrom))]
        private DateTime? _borrowalDateTo;

        [ObservableProperty]
        [SameReference(nameof(MaintenanceDateTo))]
        [EarlierThan(nameof(MaintenanceDateTo))]
        private DateTime? _maintenanceDateFrom;

        [ObservableProperty]
        [SameReference(nameof(MaintenanceDateFrom))]
        private DateTime? _maintenanceDateTo;

        [ObservableProperty]
        [SameReference(nameof(InvoiceDateTo))]
        [EarlierThan(nameof(InvoiceDateTo))]
        private DateTime? _invoiceDateFrom;

        [ObservableProperty]
        [SameReference(nameof(InvoiceDateFrom))]
        private DateTime? _invoiceDateTo;

        [ObservableProperty]
        private bool _borrowalDateRangeEnabled = false;
        [ObservableProperty]
        private bool _maintenanceDateRangeEnabled = false;
        [ObservableProperty]
        private bool _invoiceDateRangeEnabled = false;
        [ObservableProperty]
        private bool _submitButtonEnabled = false;

        [ObservableProperty]
        private IAsyncRelayCommand? _submitCommand;

        public void ValidateAll()
        {
            this.ClearErrors();
            if (BorrowalChecked)
            {
                this.ValidateProperty(BorrowalDateFrom, nameof(BorrowalDateFrom));
                this.ValidateProperty(BorrowalDateTo, nameof(BorrowalDateTo));
            }
            if (MaintenanceChecked)
            {
                this.ValidateProperty(MaintenanceDateFrom, nameof(MaintenanceDateFrom));
                this.ValidateProperty(MaintenanceDateTo, nameof(MaintenanceDateTo));
            }
            if (InvoiceChecked)
            {
                this.ValidateProperty(InvoiceDateFrom, nameof(InvoiceDateFrom));
                this.ValidateProperty(InvoiceDateTo, nameof(InvoiceDateTo));
            }
        }

        private void ToggleSubmitButtonEnabled()
        {
            if (BorrowalChecked
                    || LiftChecked
                    || MaintenanceChecked
                    || InvoiceChecked
                    || CustomerChecked)
                SubmitButtonEnabled = true;
            else
                SubmitButtonEnabled = false;
        }
    }
}
