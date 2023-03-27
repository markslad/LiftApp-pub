using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiftApp.Dal.Interfaces;
using LiftApp.Dal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LiftApp.ViewModels
{
    public partial class NewBorrowalViewModel : ObservableValidator
    {
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private DateTime? _dateFrom;

        partial void OnDateFromChanged(DateTime? value)
        {
            GetAvailableLiftsCommand?.Execute(null);
        }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        [CustomValidation(typeof(NewBorrowalViewModel), nameof(ValidateDateToBiggerOrEqualThanDateFrom))]
        private DateTime? _dateTo;

        partial void OnDateToChanged(DateTime? value)
        {
            GetAvailableLiftsCommand?.Execute(null);
        }

        [ObservableProperty]
        private ObservableCollection<Lift>? _lifts;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private Lift? _selectedLift;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        [RegularExpression("^([1-9][0-9]*)$", ErrorMessage = "Neplatný formát - celé číslo")]
        private string? _priceADay;

        [ObservableProperty]
        private ObservableCollection<Customer>? _customers;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private Customer? _selectedCustomer;


        [ObservableProperty]
        private bool _isNewCustomer = false;


        [ObservableProperty]
        private IAsyncRelayCommand? _getAvailableLiftsCommand;

        [ObservableProperty]
        private ICommand? _addNewCustomerCommand;

        [ObservableProperty]
        private ICommand? _closeCommand;

        [ObservableProperty]
        private IAsyncRelayCommand? _submitCommand;

        public NewBorrowalViewModel()
        {
        }

        public void ValidateAll()
        {
            this.ClearErrors();
            this.ValidateAllProperties();
        }

        public bool IsDatesValid()
        {
            this.ClearErrors();
            this.ValidateProperty(DateFrom, nameof(DateFrom));
            this.ValidateProperty(DateTo, nameof(DateTo));
            if (this.HasErrors) return false;
            else return true;
        }

        public static ValidationResult ValidateDateToBiggerOrEqualThanDateFrom(DateTime? dateTo, ValidationContext context)
        {
            var dateFrom = ((NewBorrowalViewModel)context.ObjectInstance).DateFrom;
            if (dateFrom is null || dateTo is null)
                return new("Datum od a Datum do jsou povinné údaje");

            bool isValid = dateTo >= dateFrom;
            if (isValid) return ValidationResult.Success!;
            else return new("Neplatný údaj (dříve než Datum do)");
        }
    }
}
