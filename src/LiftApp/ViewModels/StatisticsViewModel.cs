using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiftApp.Statistics.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LiftApp.ViewModels
{
    public partial class StatisticsViewModel : ObservableObject
    {
        [ObservableProperty]
        private OverallIncomeByDaysViewModel? _overallIncomeByDaysViewModel;

        [ObservableProperty]
        private IncomeByCustomerViewModel? _incomeByCustomerViewModel;

        [ObservableProperty]
        private BorrowedLiftTypeByDaysViewModel? _borrowedLiftTypeByDaysViewModel;

        [ObservableProperty]
        private IncomeYearComparisonViewModel? _incomeYearComparisonViewModel;

        [ObservableProperty]
        private IEnumerable<BorrowalReportType> _borrowalReportTypes = new []
        {
            BorrowalReportType.OverallIncomeByDays,
            BorrowalReportType.IncomeByCustomers,
            BorrowalReportType.BorrowedLiftTypeByDays,
            BorrowalReportType.IncomeYearComparison
        };

        [ObservableProperty]
        private BorrowalReportType? _selectedBorrowalReportType;

        partial void OnSelectedBorrowalReportTypeChanged(BorrowalReportType? value)
        {
            CurrentViewModel = value switch
            {
                BorrowalReportType.OverallIncomeByDays => OverallIncomeByDaysViewModel,
                BorrowalReportType.IncomeByCustomers => IncomeByCustomerViewModel,
                BorrowalReportType.BorrowedLiftTypeByDays => BorrowedLiftTypeByDaysViewModel,
                BorrowalReportType.IncomeYearComparison => IncomeYearComparisonViewModel,
                _ => throw new NotImplementedException("Unknown report type")
            };
        }

        [ObservableProperty]
        private ObservableObject? _currentViewModel;
    }
}
