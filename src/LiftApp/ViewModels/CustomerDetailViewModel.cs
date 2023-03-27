using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LiftApp.ViewModels
{
    public partial class CustomerDetailViewModel : ObservableObject
    {
        [ObservableProperty]
        private ICommand? _closeCommand;
        [ObservableProperty]
        private ObservableCollection<string>? _keys;
        [ObservableProperty]
        private ObservableCollection<string>? _values;
    }
}
