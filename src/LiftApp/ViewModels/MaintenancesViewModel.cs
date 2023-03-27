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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace LiftApp.ViewModels
{
    public partial class MaintenancesViewModel : ObservableObject
    {
        [ObservableProperty]
        private ICommand? _showLiftDetailCommand;

        [ObservableProperty]
        private ObservableCollection<Maintenance>? _maintenances;

        partial void OnMaintenancesChanged(ObservableCollection<Maintenance>? value)
        {
            MaintenancesView = CollectionViewSource.GetDefaultView(value);
        }

        [ObservableProperty]
        private ICollectionView? _maintenancesView;

        [ObservableProperty]
        private Maintenance? _selectedMaintenance;

        partial void OnSelectedMaintenanceChanged(Maintenance? value)
        {
            if(value is null)
            {
                EditButtonEnabled = false;
                DeleteButtonEnabled = false;
            }
            else
            {
                EditButtonEnabled = true;
                DeleteButtonEnabled = true;
            }
        }

        [ObservableProperty]
        private string? _filter;

        partial void OnFilterChanged(string? value)
        {
            if (Maintenances is null)
                throw new NullReferenceException("Maintenances items source is null");

            MaintenancesView = CollectionViewSource.GetDefaultView(Maintenances);
            if (value is null)
                return;

            MaintenancesView.Filter = o =>
            {
                var m = (Maintenance)o;
                return m.Lift.SerialNumber.Contains(value);
            };
        }

        [ObservableProperty]
        private IAsyncRelayCommand<Maintenance?>? _showEditMaintenanceCommand;

        [ObservableProperty]
        private IAsyncRelayCommand<Maintenance>? _deleteMaintenanceCommand;

        [ObservableProperty]
        private bool _deleteButtonEnabled = false;

        [ObservableProperty]
        private bool _editButtonEnabled = false;
    }
}
