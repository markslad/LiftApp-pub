using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.ViewModels
{
    public partial class InfoBarViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? _text;
    }
}
