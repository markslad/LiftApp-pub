using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftApp.Enums;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;

namespace LiftApp.ViewModels
{
    public partial class NewCustomerViewModel : ObservableValidator
    {
        [ObservableProperty]
        private IEnumerable<CustomerType> _customerTypes = new CustomerType[]
        {
            CustomerType.NonEntrepreneurCustomer,
            CustomerType.OwnAccountWorker,
            CustomerType.LegalEntity
        };

        [ObservableProperty]
        [Required(ErrorMessage = "Vyberte typ zákazníka")]
        private CustomerType? _selectedCustomerType;

        partial void OnSelectedCustomerTypeChanged(CustomerType? value)
        {
            switch (value)
            {
                case null:
                    IsIdentifierVisible = false;
                    IsTaxIdentificationNumberVisible = false;
                    IsFirstNameVisible = false;
                    IsSurnameVisible = false;
                    IsNameVisible = false;
                    IsEmailVisible = false;
                    IsPhoneNumberVisible = false;
                    SubmitEnabled = false;
                    break;
                case CustomerType.NonEntrepreneurCustomer:
                    IsIdentifierVisible = true;
                    IsTaxIdentificationNumberVisible = false;
                    IsFirstNameVisible = true;
                    IsSurnameVisible = true;
                    IsNameVisible = false;
                    IsEmailVisible = true;
                    IsPhoneNumberVisible = true;
                    SubmitEnabled = true;
                    ValidateAll = ValidateNonEntrepreneurCustomer;
                    break;
                case CustomerType.OwnAccountWorker:
                    IsIdentifierVisible = true;
                    IsTaxIdentificationNumberVisible = true;
                    IsFirstNameVisible = true;
                    IsSurnameVisible = true;
                    IsNameVisible = false;
                    IsEmailVisible = true;
                    IsPhoneNumberVisible = true;
                    SubmitEnabled = true;
                    ValidateAll = ValidateOwnAccountWorker;
                    break;
                case CustomerType.LegalEntity:
                    IsIdentifierVisible = true;
                    IsTaxIdentificationNumberVisible = true;
                    IsFirstNameVisible = false;
                    IsSurnameVisible = false;
                    IsNameVisible = true;
                    IsEmailVisible = true;
                    IsPhoneNumberVisible = true;
                    SubmitEnabled = true;
                    ValidateAll = ValidateLegalEntity;
                    break;
                default:
                    throw new NotImplementedException($"Unknown customer type in New Customer Form.");
            }
        }

        // Customer properties
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private string? _identifier;

        [ObservableProperty]
        private string? _taxIdentificationNumber;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private string? _firstName;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private string? _surname;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private string? _name;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        [RegularExpression("^[+]?[()/0-9. -]{9,}$", ErrorMessage = "Neplatné telefonní číslo")]
        private string? _phoneNumber;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Neplatný email")]
        private string? _email;

        // Address properties
        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private string? _street;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        [RegularExpression(@"^[1-9]+[0-9]*$", ErrorMessage = "Neplatný údaj")]
        private string? _houseNumber;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private string? _city;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        [RegularExpression(@"^[0-9]{3}\s?[0-9]+$", ErrorMessage = "Neplatný údaj")]
        private string? _zipCode;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private string? _country;

        // Visibility properties
        [ObservableProperty]
        private bool _isIdentifierVisible = false;

        [ObservableProperty]
        private bool _isTaxIdentificationNumberVisible = false;

        [ObservableProperty]
        private bool _isFirstNameVisible = false;

        [ObservableProperty]
        private bool _isSurnameVisible = false;

        [ObservableProperty]
        private bool _isNameVisible = false;

        [ObservableProperty]
        private bool _isEmailVisible = false;

        [ObservableProperty]
        private bool _isPhoneNumberVisible = false;

        [ObservableProperty]
        private bool _submitEnabled = false;



        // Commands
        [ObservableProperty]
        private ICommand? _closeCommand;

        [ObservableProperty]
        private ICommand? _submitCommand;

        // Delegates
        public delegate void ValidateDelegate();

        public ValidateDelegate ValidateAll = () => { return; };

        // Methods
        private void ValidateNonEntrepreneurCustomer()
        {
            this.ClearErrors();
            this.ValidateProperty(Identifier, nameof(Identifier));
            this.ValidateProperty(FirstName, nameof(FirstName));
            this.ValidateProperty(Surname, nameof(Surname));
            this.ValidateProperty(PhoneNumber, nameof(PhoneNumber));
            this.ValidateProperty(Email, nameof(Email));
            ValidateAddress();
        }

        private void ValidateOwnAccountWorker()
        {
            this.ClearErrors();
            this.ValidateProperty(Identifier, nameof(Identifier));
            this.ValidateProperty(TaxIdentificationNumber, nameof(TaxIdentificationNumber));
            this.ValidateProperty(FirstName, nameof(FirstName));
            this.ValidateProperty(Surname, nameof(Surname));
            this.ValidateProperty(PhoneNumber, nameof(PhoneNumber));
            this.ValidateProperty(Email, nameof(Email));
            ValidateAddress();
        }

        private void ValidateLegalEntity()
        {
            this.ClearErrors();
            this.ValidateProperty(Identifier, nameof(Identifier));
            this.ValidateProperty(TaxIdentificationNumber, nameof(TaxIdentificationNumber));
            this.ValidateProperty(Name, nameof(Name));
            this.ValidateProperty(PhoneNumber, nameof(PhoneNumber));
            this.ValidateProperty(Email, nameof(Email));
            ValidateAddress();
        }

        private void ValidateAddress()
        {
            this.ValidateProperty(Street, nameof(Street));
            this.ValidateProperty(HouseNumber, nameof(HouseNumber));
            this.ValidateProperty(City, nameof(City));
            this.ValidateProperty(ZipCode, nameof(ZipCode));
            this.ValidateProperty(Country, nameof(Country));
        }
    }
}
