using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiftApp.Dal.Enums;
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
    public partial class EditLiftViewModel : ObservableValidator
    {
        [ObservableProperty]
        private string? _caption;

        [ObservableProperty]
        private Lift? _liftToEdit;

        [ObservableProperty]
        private bool _serialNumberEnabled = false;

        partial void OnLiftToEditChanged(Lift? value)
        {
            if (value is not null)
            {
                SerialNumber = value.SerialNumber;
                Manufacturer = value.Manufacturer;
                MaximumHeight = value.MaximumHeight.ToString();
                SelectedPowerSource = value.PowerSource;
                SelectedOffice = value.Office;
            }
        }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Neplatný formát")]
        private string? _serialNumber;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private string? _manufacturer;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        [RegularExpression("^[1-9]{1}[0-9]{0,1}$", ErrorMessage = "Neplatný formát")]
        private string? _maximumHeight;

        [ObservableProperty]
        private ObservableCollection<PowerSource> _powerSources = new() { PowerSource.Diesel, PowerSource.Electric };

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private PowerSource? _selectedPowerSource;

        [ObservableProperty]
        private ObservableCollection<Office>? _offices;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private Office? _selectedOffice;

        [ObservableProperty]
        private IAsyncRelayCommand? _closeCommand;

        [ObservableProperty]
        private ICommand? _submitCommand;

        public void ValidateAll()
        {
            this.ClearErrors();
            this.ValidateAllProperties();
        }
    }
}
