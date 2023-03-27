using CommunityToolkit.Mvvm.ComponentModel;
using LiftApp.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LiftApp.ViewModels
{
    public partial class LiftDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private ICommand? _closeCommand;
        [ObservableProperty]
        private Lift? _lift;
    }
}
