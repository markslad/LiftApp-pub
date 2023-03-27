using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.ViewModels
{
    public partial class BorrowedLiftTypeByDaysViewModel : ObservableObject
    {
        [ObservableProperty]
        private ISeries[]? _series;

        [ObservableProperty]
        private Axis[]? _xAxes;
    }
}
