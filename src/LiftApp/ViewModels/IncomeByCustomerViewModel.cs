using CommunityToolkit.Mvvm.ComponentModel;
using LiftApp.Dal.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.ViewModels
{
    public partial class IncomeByCustomerViewModel : ObservableObject
    {
        [ObservableProperty]
        private ISeries[]? _series;

        partial void OnSeriesChanged(ISeries[]? value)
        {
            FilteredSeries = value;
        }

        [ObservableProperty]
        private ISeries[]? _filteredSeries;

        [ObservableProperty]
        private Axis[]? _xAxes;

        [ObservableProperty]
        private ObservableCollection<Customer>? _customers;

        [ObservableProperty]
        private Customer? _selectedCustomer;

        partial void OnSelectedCustomerChanged(Customer? value)
        {
            if(Series is not null && value is not null)
            {
                FilteredSeries = Series.Where(s => s.Name == value.Identifier).ToArray();
                ChartVisible = true;
            }
            else FilteredSeries = Series;
        }

        [ObservableProperty]
        private bool _chartVisible = false;
    }
}
