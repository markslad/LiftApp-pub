using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiftApp.Dal.Models;
using LiftApp.Options;
using LiftApp.Views;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.ViewModels
{
    public partial class NewInvoiceViewModel : ObservableValidator
    {
        private readonly IOptions<NewInvoiceOptions> _options;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private string? _identifier;

        [ObservableProperty]
        private ObservableCollection<Borrowal>? _borrowals;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj")]
        private Borrowal? _selectedBorrowal;

        partial void OnSelectedBorrowalChanged(Borrowal? value)
        {
            if(value is not null)
            {
                UpdatePrice(value);
                DateOfTaxableSupply = value.TimeInterval.DateTo.ToDateTime(TimeOnly.Parse("00:00"));
            }
        }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        [CustomValidation(typeof(NewInvoiceViewModel), nameof(ValidateDateOfIssueIsAfterEndOfBorrowal))]
        private DateTime? _dateOfIssue;

        partial void OnDateOfIssueChanged(DateTime? value)
        {
            if (value is not null)
                DueDate = value.Value.AddDays(_options.Value.NumberOfDaysAfterDateOfIssueForDueDate);
        }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        [CustomValidation(typeof(NewInvoiceViewModel), nameof(ValidateDueDateIsAfterDateOfIssue))]
        private DateTime? _dueDate;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private DateTime? _dateOfTaxableSupply;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        [RegularExpression("^[1-9]{1}[0-9]*$", ErrorMessage = "Neplatný formát")]
        private string? _price;

        partial void OnPriceChanged(string? value)
        {
            UpdatePriceWithValueAddedTax();
        }

        [ObservableProperty]
        private bool? _isExtra;

        partial void OnIsExtraChanged(bool? value)
        {
            if (value is null)
                throw new NullReferenceException("IsExtra value is null");
            if(value == true)
            {
                IsPriceReadOnly = false;
                Price = null;
                ValueAddedTaxRate = _options.Value.ExtraInvoiceValueAddedTaxRate.ToString();
            }
            else
            {
                IsPriceReadOnly = true;
                ValueAddedTaxRate = _options.Value.ValueAddedTaxRate.ToString();
                if(SelectedBorrowal is not null)
                {
                    UpdatePrice(SelectedBorrowal);
                }
            }

            UpdatePriceWithValueAddedTax();
        }

        [ObservableProperty]
        private bool _isPriceReadOnly = true;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private string? _valueAddedTaxRate;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private string? _priceWithValueAddedTax;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private string? _bank;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private string? _bankAccount;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private string? _variableSymbol;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private IAsyncRelayCommand? _closeCommand;

        [ObservableProperty]
        private IAsyncRelayCommand? _submitCommand;

        public NewInvoiceViewModel(IOptions<NewInvoiceOptions> options)
        {
            _options = options;
            Bank = options.Value.Bank;
            BankAccount = options.Value.BankAccount;
            IsExtra = false;
        }

        public void ValidateAll()
        {
            this.ClearErrors();
            this.ValidateAllProperties();
        }

        private void UpdatePrice(Borrowal borrowal) 
        {
            Price = (borrowal.PriceADay * (borrowal.TimeInterval.DateTo.DayNumber - borrowal.TimeInterval.DateFrom.DayNumber + 1)).ToString();
        }

        private void UpdatePriceWithValueAddedTax()
        {
            if (Price is null)
            {
                PriceWithValueAddedTax = null;
            }
            else if(float.TryParse(Price, out float parsedPrice))
            {
                PriceWithValueAddedTax = (parsedPrice * (1 + float.Parse(ValueAddedTaxRate!))).ToString();
            }
            else
            {
                PriceWithValueAddedTax = null;
            }
        }

        public static ValidationResult ValidateDateOfIssueIsAfterEndOfBorrowal(DateTime? dateOfIssue, ValidationContext context)
        {
            var selectedBorrowal = ((NewInvoiceViewModel)context.ObjectInstance).SelectedBorrowal;
            if(selectedBorrowal is null)
                return new("Je nutné vybrat výpůjčku");
            if (dateOfIssue is null)
                return new("Datum vystavení je povinný údaj");

            bool isValid = DateOnly.FromDateTime(dateOfIssue.Value) >= selectedBorrowal.TimeInterval.DateTo;
            if (isValid) return ValidationResult.Success!;
            else return new("Údaj nesmí být dříve než konec výpůjčky");
        }

        public static ValidationResult ValidateDueDateIsAfterDateOfIssue(DateTime? dueDate, ValidationContext context)
        {
            var dateOfIssue = ((NewInvoiceViewModel)context.ObjectInstance).DateOfIssue;
            if (dateOfIssue is null)
                return new("Je nutné vyplnit Datum vystavení");
            if (dueDate is null)
                return new("Datum splatnosti je povinný údaj");

            bool isValid = dueDate >= dateOfIssue;
            if (isValid) return ValidationResult.Success!;
            else return new("Datum splatnosti je před datem vystavení");
        }
    }
}
