using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiftApp.Dal.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LiftApp.ViewModels
{
    public partial class OverallIncomeByDaysViewModel : ObservableObject
    {
        [ObservableProperty]
        private ISeries[]? _series;

        [ObservableProperty]
        private Axis[]? _xAxes;
    }
}
