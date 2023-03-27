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
    public partial class BorrowalDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private ICommand? _closeCommand;
        [ObservableProperty]
        private Borrowal? _borrowal;
    }
}
