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
    public partial class EditMaintenanceViewModel : ObservableValidator
    {
        [ObservableProperty]
        private Maintenance? _maintenanceToEdit;

        [ObservableProperty]
        private string? _caption;

        partial void OnMaintenanceToEditChanged(Maintenance? value)
        {
            if(value != null)
            {
                Lifts = new(new Lift[] { value.Lift });
                SelectedLift = value.Lift;
                DateFrom = value.TimeInterval.DateFrom.ToDateTime(TimeOnly.Parse("00:00"));
                DateTo = value.TimeInterval.DateTo.ToDateTime(TimeOnly.Parse("00:00"));
                Description = value.Description;
                Price = value.Price.ToString();
            }
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
        private DateTime? _dateFrom;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        [CustomValidation(typeof(EditMaintenanceViewModel), nameof(ValidateDateToBiggerOrEqualThanDateFrom))]
        private DateTime? _dateTo;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private string? _description;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [RegularExpression("^[1-9]{1}[0-9]*$", ErrorMessage = "Neplatný formát")]
        private string? _price;

        partial void OnPriceChanged(string? value)
        {
            if(value is not null)
            {
                if(value == string.Empty)
                {
                    Price = null;
                }
            }
        }

        [ObservableProperty]
        private IAsyncRelayCommand? _closeCommand;

        [ObservableProperty]
        private IAsyncRelayCommand? _submitCommand;

        public void ValidateAll()
        {
            this.ClearErrors();
            this.ValidateAllProperties();
        }

        public static ValidationResult ValidateDateToBiggerOrEqualThanDateFrom(DateTime? _dateTo, ValidationContext context)
        {
            var dateFrom = ((EditMaintenanceViewModel)context.ObjectInstance).DateFrom;
            if (dateFrom is null || _dateTo is null)
                return new("Datum od a Datum do jsou povinné údaje");

            bool isValid = _dateTo >= dateFrom;
            if (isValid) return ValidationResult.Success!;
            else return new("Neplatný údaj (dříve než Datum do)");
        }
    }
}
