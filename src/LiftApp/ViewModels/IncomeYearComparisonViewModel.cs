using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
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
    public partial class IncomeYearComparisonViewModel : ObservableValidator
    {
        [ObservableProperty]
        private ISeries[]? _series;

        [ObservableProperty]
        private Axis[]? _xAxes;

        [ObservableProperty]
        private ObservableCollection<int>? _years;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        private int? _selectedFirstYear;

        partial void OnSelectedFirstYearChanged(int? value)
        {
            UpdateReportCommand!.Execute(null);
        }

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Povinný údaj", AllowEmptyStrings = false)]
        [CustomValidation(typeof(IncomeYearComparisonViewModel), nameof(ValidateSecondYearBiggerThanFirstYear))]
        private int? _selectedSecondYear;

        partial void OnSelectedSecondYearChanged(int? value)
        {
            UpdateReportCommand!.Execute(null);
        }

        [ObservableProperty]
        private ICommand? _updateReportCommand;

        public static ValidationResult ValidateSecondYearBiggerThanFirstYear(int? selectedSecondYear, ValidationContext context)
        {
            var selectedFirstYear = ((IncomeYearComparisonViewModel)context.ObjectInstance).SelectedFirstYear;
            if (selectedFirstYear is null || selectedSecondYear is null)
                return new("První rok a druhý rok jsou povinné údaje");

            bool isValid = selectedFirstYear < selectedSecondYear;
            if (isValid) return ValidationResult.Success!;
            else return new("Druhý rok musí být později než první rok");
        }

        public void ValidateAll()
        {
            this.ValidateAllProperties();
        }
    }
}
