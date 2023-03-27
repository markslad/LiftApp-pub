using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiftApp.Dal.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace LiftApp.ViewModels
{
    public partial class LiftsViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Lift>? _lifts;

        partial void OnLiftsChanged(ObservableCollection<Lift>? value)
        {
            LiftsView = CollectionViewSource.GetDefaultView(value);
        }

        [ObservableProperty]
        private ICollectionView? _liftsView;

        [ObservableProperty]
        private Lift? _selectedLift;

        partial void OnSelectedLiftChanged(Lift? value)
        {
            if (value is null)
            {
                ChangeLiftStateEnabled = false;
                ChangeLiftStateCaption = "Vyřadit/Zrušit vyřazení";
                EditLiftEnabled = false;
            }
            else if (value.Eliminated == true)
            {
                ChangeLiftStateEnabled = true;
                ChangeLiftStateCaption = "Zrušit vyřazení";
                EditLiftEnabled = true;
            }
            else
            {
                ChangeLiftStateEnabled = true;
                ChangeLiftStateCaption = "Vyřadit";
                EditLiftEnabled = true;
            }
        }

        [ObservableProperty]
        private bool _changeLiftStateEnabled = false;

        [ObservableProperty]
        private bool _editLiftEnabled = false;

        [ObservableProperty]
        private string? _changeLiftStateCaption = "Vyřadit/Zrušit vyřazení";

        [ObservableProperty]
        private IAsyncRelayCommand? _changeLiftStateCommand;

        [ObservableProperty]
        private IAsyncRelayCommand? _showNewLiftCommand;

        [ObservableProperty]
        private IAsyncRelayCommand? _showEditLiftCommand;

        [ObservableProperty]
        private string? _filter;

        partial void OnFilterChanged(string? value)
        {
            if (Lifts is null)
                throw new NullReferenceException("Lifts items source is null");

            LiftsView = CollectionViewSource.GetDefaultView(Lifts);
            if (value is null)
                return;

            LiftsView.Filter = o =>
            {
                var l = (Lift)o;
                return l.SerialNumber.Contains(value);
            };
        }
    }
}
