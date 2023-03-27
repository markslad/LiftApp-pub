using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiftApp.Dal;
using LiftApp.Dal.Interfaces;
using LiftApp.Dal.Models;
using LiftApp.Export.Enums;
using LiftApp.Export.Models;
using LiftApp.Export.Services;
using LiftApp.Statistics.Enums;
using LiftApp.Statistics.Factories;
using LiftApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LiftApp.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private IServiceProvider _serviceProvider;
        private readonly ILogger<MainWindowViewModel> _logger;
        private readonly IMainUnitOfWork _mainUnitOfWork;
        private WaitingView? _waitingView;

        [ObservableProperty]
        private ObservableObject? _currentViewModel;
        [ObservableProperty]
        private BorrowalsViewModel _borrowalsViewModel;
        [ObservableProperty]
        private LiftDetailViewModel _liftDetailViewModel;
        [ObservableProperty]
        private CustomerDetailViewModel _customerDetailViewModel;
        [ObservableProperty]
        private LiftsViewModel _liftsViewModel;
        [ObservableProperty]
        private CustomersViewModel _customersViewModel;
        [ObservableProperty]
        private InvoicesViewModel _invoicesViewModel;
        [ObservableProperty]
        private BorrowalDetailViewModel _borrowalDetailViewModel;
        [ObservableProperty]
        private MaintenancesViewModel _maintenancesViewModel;
        [ObservableProperty]
        private NewBorrowalViewModel? _newBorrowalViewModel;
        [ObservableProperty]
        private NewCustomerViewModel? _newCustomerViewModel;
        [ObservableProperty]
        private EditLiftViewModel? _editLiftViewModel;
        [ObservableProperty]
        private NewInvoiceViewModel? _newInvoiceViewModel;
        [ObservableProperty]
        private EditMaintenanceViewModel? _editMaintenanceViewModel;
        [ObservableProperty]
        private ExportViewModel? _exportViewModel;
        [ObservableProperty]
        private InfoBarViewModel _infoBarViewModel;
        [ObservableProperty]
        private StatisticsViewModel? _statisticsViewModel;

        [ObservableProperty]
        private IAsyncRelayCommand _menuNavigateToBorrowalsCommand;
        [ObservableProperty]
        private IAsyncRelayCommand _menuNavigateToLiftsCommand;
        [ObservableProperty]
        private IAsyncRelayCommand _menuNavigateToCustomersCommand;
        [ObservableProperty]
        private IAsyncRelayCommand _menuNavigateToInvoicesCommand;
        [ObservableProperty]
        private ICommand _menuNavigateToExportCommand;
        [ObservableProperty]
        private IAsyncRelayCommand _menuNavigateToMaintenancesCommand;
        [ObservableProperty]
        private IAsyncRelayCommand _menuNavigateToStatisticsCommand;

        [ObservableProperty]
        private IAsyncRelayCommand _windowLoadedCommand;
        [ObservableProperty]
        private bool _infoBarVisible = false;
        [ObservableProperty]
        private string? _appVersion;

        public MainWindowViewModel(IServiceProvider serviceProvider)
        {
            AppVersion = Assembly.GetExecutingAssembly().GetName().Version!.ToString();

            // Services
            _serviceProvider = serviceProvider;
            _logger = _serviceProvider.GetRequiredService<ILogger<MainWindowViewModel>>();
            _mainUnitOfWork = _serviceProvider.GetRequiredService<IMainUnitOfWork>();

            // ViewModels
            _borrowalsViewModel = _serviceProvider.GetRequiredService<BorrowalsViewModel>();
            _liftDetailViewModel = _serviceProvider.GetRequiredService<LiftDetailViewModel>();
            _customerDetailViewModel = _serviceProvider.GetRequiredService<CustomerDetailViewModel>();
            _liftsViewModel = _serviceProvider.GetRequiredService<LiftsViewModel>();
            _customersViewModel = _serviceProvider.GetRequiredService<CustomersViewModel>();
            _invoicesViewModel = _serviceProvider.GetRequiredService<InvoicesViewModel>();
            _borrowalDetailViewModel = _serviceProvider.GetRequiredService<BorrowalDetailViewModel>();
            _maintenancesViewModel = _serviceProvider.GetRequiredService<MaintenancesViewModel>();
            _infoBarViewModel = _serviceProvider.GetRequiredService<InfoBarViewModel>();

            // Commands
            _menuNavigateToBorrowalsCommand = new AsyncRelayCommand(MenuNavigateToBorrowalsAsync);
            _menuNavigateToLiftsCommand = new AsyncRelayCommand(MenuNavigateToLiftsAsync);
            _menuNavigateToCustomersCommand = new AsyncRelayCommand(MenuNavigateToCustomersAsync);
            _menuNavigateToInvoicesCommand = new AsyncRelayCommand(MenuNavigateToInvoicesAsync);
            _menuNavigateToMaintenancesCommand = new AsyncRelayCommand(MenuNavigateToMaintenancesAsync);
            _windowLoadedCommand = new AsyncRelayCommand(MenuNavigateToBorrowalsAsync);
            _menuNavigateToExportCommand = new RelayCommand(MenuNavigateToExportAsync);
            _menuNavigateToStatisticsCommand = new AsyncRelayCommand(MenuNavigateToStatisticsAsync);

            BorrowalsViewModel.ShowLiftDetailCommand = new RelayCommand<Lift?>(BorrowalsShowLiftDetail);
            BorrowalsViewModel.ShowCustomerDetailCommand = new RelayCommand<Customer?>(BorrowalsShowCustomerDetail);
            BorrowalsViewModel.ShowNewBorrowalCommand = new AsyncRelayCommand(BorrowalsShowNewBorrowalAsync);
            BorrowalsViewModel.DeleteBorrowalCommand = new AsyncRelayCommand(BorrowalsDeleteBorrowalAsync);
            BorrowalsViewModel.ShowNewInvoiceCommand = new AsyncRelayCommand(BorrowalsShowNewInvoiceAsync);

            InvoicesViewModel.ShowBorrowalDetailCommand = new RelayCommand<Borrowal?>(InvoicesShowBorrowalDetail);
            InvoicesViewModel.ShowNewInvoiceCommand = new AsyncRelayCommand(InvoicesShowNewInvoiceAsync);
            InvoicesViewModel.ConfirmPaymentCommand = new AsyncRelayCommand(InvoicesConfirmPaymentAsync);
            InvoicesViewModel.DeleteInvoiceCommand = new AsyncRelayCommand(InvoicesDeleteInvoiceAsync);
            InvoicesViewModel.ExportInvoiceCommand = new RelayCommand(InvoicesExportInvoice);

            MaintenancesViewModel.ShowLiftDetailCommand = new RelayCommand<Lift?>(MaintenancesShowLiftDetail);
            MaintenancesViewModel.DeleteMaintenanceCommand = new AsyncRelayCommand<Maintenance>(MaintenancesDeleteMaintenanceAsync);
            MaintenancesViewModel.ShowEditMaintenanceCommand = new AsyncRelayCommand<Maintenance?>(MaintenancesShowEditMaintenanceAsync);

            CustomerDetailViewModel.CloseCommand = new RelayCommand(CustomerDetailNavigateBackToBorrowals);

            BorrowalDetailViewModel.CloseCommand = new RelayCommand(BorrowalDetailNavigateBackToInvoices);

            LiftsViewModel.ChangeLiftStateCommand = new AsyncRelayCommand(LiftsChangeLiftStateAsync);
            LiftsViewModel.ShowNewLiftCommand = new AsyncRelayCommand(LiftsShowNewLiftAsync);
            LiftsViewModel.ShowEditLiftCommand = new AsyncRelayCommand(LiftsShowEditLiftAsync);
        }

        private async Task MenuNavigateToBorrowalsAsync()
        {
            try
            {
                ShowWaitingView();

                var borrowals = await _mainUnitOfWork.BorrowalRepository.GetAsync(include: 
                    borrowals => borrowals
                    .Include(b => b.TimeInterval)
                    .Include(b => b.Customer).ThenInclude(c => c.Address)
                    .Include(b => b.Lift)
                    .Include(b => b.Invoices),
                    orderBy: borrowals => borrowals.OrderByDescending(borrowal => borrowal.TimeInterval.DateFrom));
                BorrowalsViewModel.Borrowals = new ObservableCollection<Dal.Models.Borrowal>(borrowals);
                CurrentViewModel = BorrowalsViewModel;

                CloseWaitingView();
                _logger.LogInformation("Navigated to Borrowals view");
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                ShowErrorWindow(ex.Message);
                _logger.LogError(ex, "");
            } 
        }

        private void BorrowalsShowLiftDetail(Lift? lift)
        {
            if (lift == null) throw new ArgumentNullException("Lift is null");
            LiftDetailViewModel.Lift = lift;
            LiftDetailViewModel.CloseCommand = new RelayCommand(LiftDetailNavigateBackToBorrowals);
            CurrentViewModel = LiftDetailViewModel;
            _logger.LogInformation("Navigated from Borrowals to Lift detail");
        }

        private void BorrowalsShowCustomerDetail(Customer? customer)
        {
            if (customer == null) throw new ArgumentNullException();
            CustomerDetailViewModel.Keys = new ObservableCollection<string>();
            CustomerDetailViewModel.Values = new ObservableCollection<string>();

            CustomerDetailViewModel.Keys.Add("Identifikátor");
            CustomerDetailViewModel.Values.Add(customer.Identifier);
            if (customer is EntrepreneurCustomer)
            {
                CustomerDetailViewModel.Keys.Add("DIČ");
                CustomerDetailViewModel.Values.Add(((EntrepreneurCustomer)customer).TaxIdentificationNumber ?? String.Empty);
            }
            if (customer is NonEntrepreneurCustomer)
            {
                CustomerDetailViewModel.Keys.Add("Jméno");
                CustomerDetailViewModel.Keys.Add("Příjmení");
                CustomerDetailViewModel.Values.Add(((NonEntrepreneurCustomer)customer).FirstName);
                CustomerDetailViewModel.Values.Add(((NonEntrepreneurCustomer)customer).Surname);
            }
            if (customer is OwnAccountWorker)
            {
                CustomerDetailViewModel.Keys.Add("Jméno");
                CustomerDetailViewModel.Keys.Add("Příjmení");
                CustomerDetailViewModel.Values.Add(((OwnAccountWorker)customer).FirstName);
                CustomerDetailViewModel.Values.Add(((OwnAccountWorker)customer).Surname);
            }
            if (customer is LegalEntity)
            {
                CustomerDetailViewModel.Keys.Add("Název");
                CustomerDetailViewModel.Values.Add(((LegalEntity)customer).Name);
            }
            CustomerDetailViewModel.Keys.Add("Telefonní číslo");
            CustomerDetailViewModel.Values.Add(customer.PhoneNumber);
            CustomerDetailViewModel.Keys.Add("Email");
            CustomerDetailViewModel.Values.Add(customer.Email);
            CustomerDetailViewModel.Keys.Add("Adresa");
            CustomerDetailViewModel.Values.Add($"{customer.Address.Street} {customer.Address.HouseNumber}, {customer.Address.City}, {customer.Address.ZipCode} {customer.Address.Country}");


            CurrentViewModel = CustomerDetailViewModel;
            _logger.LogInformation("Navigated from Borrowals to Customer detail");
        }

        private void LiftDetailNavigateBackToBorrowals()
        {
            CurrentViewModel = BorrowalsViewModel;
            _logger.LogInformation("Navigated from Lift detail to Borrowals");
        }

        private void CustomerDetailNavigateBackToBorrowals()
        {
            CurrentViewModel = BorrowalsViewModel;
            _logger.LogInformation("Navigated from Customer detail to Borrowals");
        }

        private async Task MenuNavigateToLiftsAsync()
        {
            try
            {
                ShowWaitingView();
                
                var lifts = await _mainUnitOfWork.LiftRepository.GetAsync(include:
                    lifts => lifts
                    .Include(l => l.Office).ThenInclude(o => o.Address),
                    orderBy: lifts => lifts
                        .OrderBy(lift => lift.SerialNumber.Length)
                        .ThenBy(lift => lift.SerialNumber));
                LiftsViewModel.Lifts = new ObservableCollection<Lift>(lifts);
                LiftsViewModel.SelectedLift = null;
                CurrentViewModel = LiftsViewModel;

                CloseWaitingView();
                _logger.LogInformation("Navigated to Lifts view");
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                ShowErrorWindow(ex.Message);
                _logger.LogError(ex, "");
            }
        }

        private async Task MenuNavigateToCustomersAsync()
        {
            try
            {
                ShowWaitingView();

                var customers = await _mainUnitOfWork.CustomerRepository.GetAsync(include:
                    customers => customers.Include(c => c.Address),
                    orderBy: customers => customers
                        .OrderBy(customer => customer.Identifier.Length)
                        .ThenBy(customer => customer.Identifier));
                CustomersViewModel.Customers = new ObservableCollection<Customer>(customers);
                CurrentViewModel = CustomersViewModel;
                
                CloseWaitingView();
                _logger.LogInformation("Navigated to Customers view");
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                ShowErrorWindow(ex.Message);
                _logger.LogError(ex, "");
            }
            finally
            {
                CloseWaitingView();
            }
        }

        private async Task MenuNavigateToInvoicesAsync()
        {
            try
            {
                ShowWaitingView();

                var invoices = await _mainUnitOfWork.InvoiceRepository.GetAsync(include:
                    invoices => invoices
                    .Include(i => i.Borrowal).ThenInclude(b => b.Lift).ThenInclude(b => b.Office).ThenInclude(o => o.Address)
                    .Include(i => i.Borrowal).ThenInclude(b => b.Customer).ThenInclude(c => c.Address)
                    .Include(i => i.Borrowal).ThenInclude(b => b.TimeInterval),
                    orderBy: invoices => invoices
                        .OrderBy(invoice => invoice.Identifier.Length)
                        .ThenBy(invoice => invoice.Identifier));
                InvoicesViewModel.Invoices = new ObservableCollection<Invoice>(invoices);
                CurrentViewModel = InvoicesViewModel;

                CloseWaitingView();
                _logger.LogInformation("Navigated to Invoices view");
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                ShowErrorWindow(ex.Message);
                _logger.LogError(ex, "");
            }
        }

        private void InvoicesShowBorrowalDetail(Borrowal? borrowal)
        {
            BorrowalDetailViewModel.Borrowal = borrowal;
            CurrentViewModel = BorrowalDetailViewModel;
            _logger.LogInformation("Navigated from Invoices to Borrowal detail");
        }

        private void BorrowalDetailNavigateBackToInvoices()
        {
            CurrentViewModel = InvoicesViewModel;
            _logger.LogInformation("Navigated from Borrowal detail to Invoices");
        }

        private async Task MenuNavigateToMaintenancesAsync()
        {
            try
            {
                ShowWaitingView();

                var maintenances = await _mainUnitOfWork.MaintenanceRepository.GetAsync(include:
                    maintenances => maintenances
                    .Include(m => m.TimeInterval)
                    .Include(m => m.Lift).ThenInclude(l => l.Office).ThenInclude(o => o.Address),
                    orderBy: maintenances => maintenances
                        .OrderByDescending(maintenance => maintenance.TimeInterval.DateFrom));
                MaintenancesViewModel.Maintenances = new ObservableCollection<Maintenance>(maintenances);
                CurrentViewModel = MaintenancesViewModel;

                CloseWaitingView();
                _logger.LogInformation("Navigated to Maintenances view");
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                ShowErrorWindow(ex.Message);
                _logger.LogError(ex, "");
            }
        }

        private void MaintenancesShowLiftDetail(Lift? lift)
        {
            LiftDetailViewModel.Lift = lift;
            LiftDetailViewModel.CloseCommand = new RelayCommand(LiftDetailNavigateBackToMaintenances);
            CurrentViewModel = LiftDetailViewModel;
            _logger.LogInformation("Navigated from Maintenances to Lift detail");
        }

        private void LiftDetailNavigateBackToMaintenances()
        {
            CurrentViewModel = MaintenancesViewModel;
            _logger.LogInformation("Navigated from Lift detail to Maintenances");
        }

        private async Task BorrowalsShowNewBorrowalAsync()
        {
            NewBorrowalViewModel = _serviceProvider.GetRequiredService<NewBorrowalViewModel>();
            NewBorrowalViewModel.CloseCommand = new RelayCommand(NewBorrowalNavigateBackToBorrowals);
            NewBorrowalViewModel.SubmitCommand = new AsyncRelayCommand(NewBorrowalSubmitAsync);
            NewBorrowalViewModel.GetAvailableLiftsCommand = new AsyncRelayCommand(NewBorrowalGetAvailableLiftsAsync);
            NewBorrowalViewModel.AddNewCustomerCommand = new RelayCommand(NewBorrowalAddNewCustomer);

            try
            {
                ShowWaitingView();

                var customers = await _mainUnitOfWork.CustomerRepository.GetAsync(orderBy: customers => customers
                    .OrderBy(customer => customer.Identifier));
                NewBorrowalViewModel.Customers = new ObservableCollection<Customer>(customers);
                CurrentViewModel = NewBorrowalViewModel;

                CloseWaitingView();
                _logger.LogInformation("Navigated from Borrowals to New Borrowal Form");
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                ShowErrorWindow(ex.Message);
                _logger.LogError(ex, "");
            }
        }

        private async Task BorrowalsDeleteBorrowalAsync()
        {
            var borrowalToDelete = BorrowalsViewModel.SelectedBorrowal!;
            var result = MessageBox.Show($"Opravdu chcete odstranit vybraný záznam?", "Odtranění záznamu", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                // Check that borrowal has no invoices
                ShowWaitingView();

                var invoices = await _mainUnitOfWork.InvoiceRepository.GetAsync(invoice => invoice.Borrowal == borrowalToDelete);
                CloseWaitingView();
                if (invoices.Any())
                {
                    MessageBox.Show($"Výpůjčku nelze odstranit, jelikož má evidovánu jednu nebo více faktur.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Delete borrowal
                ShowWaitingView();
                _mainUnitOfWork.TimeIntervalRepository.Delete(borrowalToDelete.TimeInterval);
                _mainUnitOfWork.BorrowalRepository.Delete(borrowalToDelete);
                await _mainUnitOfWork.SaveChangesAsync();

                CloseWaitingView();
                _logger.LogInformation($"Deleted borrowal with ID: {borrowalToDelete.Id}");
                await NotifyAboutSuccess("Úspěšně odstraněn záznam výpůjčky.");
                await MenuNavigateToBorrowalsAsync();
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                ShowErrorWindow(ex.Message);
                _logger.LogError(ex, "");
            }
        }

        private async Task BorrowalsShowNewInvoiceAsync()
        {
            var borrowal = BorrowalsViewModel.SelectedBorrowal!;
            await ShowNewInvoiceAsync(borrowal);
        }

        private void NewBorrowalNavigateBackToBorrowals()
        {
            CurrentViewModel = BorrowalsViewModel;
        }

        private async Task NewBorrowalGetAvailableLiftsAsync()
        {
            if (!NewBorrowalViewModel!.IsDatesValid())
            {
                NewBorrowalViewModel.Lifts = null;
                return;
            }
            var dateFrom = DateOnly.FromDateTime(NewBorrowalViewModel.DateFrom!.Value);
            var dateTo = DateOnly.FromDateTime(NewBorrowalViewModel.DateTo!.Value);
            IEnumerable<Lift>? lifts;

            try
            {
                ShowWaitingView();

                lifts = await _mainUnitOfWork.LiftRepository.GetAsync(include:
                    lifts => lifts
                    .Include(lift => lift.Borrowals)
                    .ThenInclude(borrowal => borrowal.TimeInterval)
                    .Include(lift => lift.Maintenances)
                    .ThenInclude(maintenance => maintenance.TimeInterval)
                    .Include(lift => lift.Office)
                    .ThenInclude(office => office.Address),
                    orderBy: lifts => lifts
                        .OrderBy(lift => lift.SerialNumber.Length)
                        .ThenBy(lift => lift.SerialNumber));

                var availableLifts = lifts
                // filter eliminated lifts
                .Where(lift => lift.Eliminated == false)
                // filter by borrowals
                .Where(lift => lift.Borrowals.TrueForAll(
                    borrowal =>
                    (dateFrom < borrowal.TimeInterval.DateFrom && dateTo < borrowal.TimeInterval.DateFrom)
                    ||
                    (dateFrom > borrowal.TimeInterval.DateTo && dateTo > borrowal.TimeInterval.DateTo)
                    )
                )
                // filter by maintenances
                .Where(lift => lift.Maintenances.TrueForAll(
                    maintenance =>
                    (dateFrom < maintenance.TimeInterval.DateFrom && dateTo < maintenance.TimeInterval.DateFrom)
                    ||
                    (dateFrom > maintenance.TimeInterval.DateTo && dateTo > maintenance.TimeInterval.DateTo)
                    )
                );

                CloseWaitingView();
                NewBorrowalViewModel.Lifts = new ObservableCollection<Lift>(availableLifts);
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private async Task NewBorrowalSubmitAsync()
        {
            NewBorrowalViewModel!.ValidateAll();
            if (NewBorrowalViewModel.HasErrors)
                return;

            var borrowal = new Borrowal()
            {
                Lift = NewBorrowalViewModel.SelectedLift!,
                Customer = NewBorrowalViewModel.SelectedCustomer!,
                PriceADay = int.Parse(NewBorrowalViewModel.PriceADay!),
                TimeInterval = new TimeInterval()
                {
                    DateFrom = DateOnly.FromDateTime(NewBorrowalViewModel.DateFrom!.Value),
                    DateTo = DateOnly.FromDateTime(NewBorrowalViewModel.DateTo!.Value),
                }
            };

            try
            {
                ShowWaitingView();

                if (NewBorrowalViewModel.IsNewCustomer)
                {
                    await _mainUnitOfWork.CustomerRepository.InsertAsync(NewBorrowalViewModel.SelectedCustomer!);
                }
                await _mainUnitOfWork.BorrowalRepository.InsertAsync(borrowal);
                await _mainUnitOfWork.SaveChangesAsync();

                CloseWaitingView();
                _logger.LogInformation($"Inserted new borrowal with id: {borrowal.Id}");
                await NotifyAboutSuccess("Úspěšně vložena nová výpůjčka.");
                await MenuNavigateToBorrowalsAsync();

                var result = MessageBox.Show("Chcete pro vytvořenou výpůjčku vytvořit fakturu?",
                    "Vytvoření faktury",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    await ShowNewInvoiceAsync(borrowal);
                }
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private async Task ShowNewInvoiceAsync(Borrowal borrowal)
        {
            try
            {
                ShowWaitingView();

                NewInvoiceViewModel = _serviceProvider.GetRequiredService<NewInvoiceViewModel>();
                NewInvoiceViewModel.CloseCommand = new AsyncRelayCommand(MenuNavigateToBorrowalsAsync);
                NewInvoiceViewModel.SubmitCommand = new AsyncRelayCommand(NewInvoiceSubmitAsync);

                // Get available Invoice identifier
                var invoices = await _mainUnitOfWork.InvoiceRepository.GetAsync();
                var identifiers = invoices.Select(i => i.Identifier);
                var parsedIdentifiers = new List<(string, string)>();
                foreach (var identifier in identifiers)
                {
                    parsedIdentifiers.Add((identifier.Split('-')[0], identifier.Split('-')[1]));
                }
                var newIdentifierPostfix = 1;
                while (identifiers.Contains($"{DateTime.Now.Year}-{newIdentifierPostfix}"))
                {
                    newIdentifierPostfix++;
                }
                NewInvoiceViewModel.Identifier = $"{DateTime.Now.Year}-{newIdentifierPostfix}";

                // Fill Variable symbol
                NewInvoiceViewModel.VariableSymbol = $"{DateTime.Now.Year}{newIdentifierPostfix}";

                // Fill Borrowals ComboBox - only with new borrowal
                NewInvoiceViewModel.Borrowals = new(new List<Borrowal>() { borrowal });
                NewInvoiceViewModel.SelectedBorrowal = borrowal;

                CloseWaitingView();
                CurrentViewModel = NewInvoiceViewModel;
                _logger.LogInformation("Navigated from New Borrowal to New invoice form");
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private void NewBorrowalAddNewCustomer()
        {
            CurrentViewModel = null;
            NewCustomerViewModel = _serviceProvider.GetRequiredService<NewCustomerViewModel>();
            NewCustomerViewModel.CloseCommand = new RelayCommand(NewCustomerCloseAndNavigateToNewBorrowal);
            NewCustomerViewModel.SubmitCommand = new RelayCommand(NewCustomerSubmitAndNavigateToNewBorrowal);
            CurrentViewModel = NewCustomerViewModel;
        }

        private void NewCustomerCloseAndNavigateToNewBorrowal()
        {
            CurrentViewModel = NewBorrowalViewModel;
        }

        private void NewCustomerSubmitAndNavigateToNewBorrowal()
        {
            NewCustomerViewModel!.ValidateAll();
            if (NewCustomerViewModel.HasErrors)
                return;

            Customer? newCustomer;

            switch (NewCustomerViewModel.SelectedCustomerType)
            {
                case Enums.CustomerType.NonEntrepreneurCustomer:
                    newCustomer = new NonEntrepreneurCustomer()
                    {
                        Identifier = NewCustomerViewModel.Identifier!,
                        FirstName = NewCustomerViewModel.FirstName!,
                        Surname = NewCustomerViewModel.Surname!,
                        PhoneNumber = NewCustomerViewModel.PhoneNumber!,
                        Email = NewCustomerViewModel.Email!,
                    };
                    break;
                case Enums.CustomerType.OwnAccountWorker:
                    newCustomer = new OwnAccountWorker()
                    {
                        Identifier = NewCustomerViewModel.Identifier!,
                        TaxIdentificationNumber = NewCustomerViewModel.TaxIdentificationNumber,
                        FirstName = NewCustomerViewModel.FirstName!,
                        Surname = NewCustomerViewModel.Surname!,
                        PhoneNumber = NewCustomerViewModel.PhoneNumber!,
                        Email = NewCustomerViewModel.Email!,
                    };
                    break;
                case Enums.CustomerType.LegalEntity:
                    newCustomer = new LegalEntity()
                    {
                        Identifier = NewCustomerViewModel.Identifier!,
                        TaxIdentificationNumber = NewCustomerViewModel.TaxIdentificationNumber,
                        Name = NewCustomerViewModel.Name!,
                        PhoneNumber = NewCustomerViewModel.PhoneNumber!,
                        Email = NewCustomerViewModel.Email!,
                    };
                    break;
                default:
                    throw new NotImplementedException("Unkonwn customer type");
            }

            newCustomer.Address = new Address()
            {
                Street = NewCustomerViewModel.Street!,
                HouseNumber = int.Parse(NewCustomerViewModel.HouseNumber!),
                City = NewCustomerViewModel.City!,
                ZipCode = NewCustomerViewModel.ZipCode!,
                Country = NewCustomerViewModel.Country!
            };

            NewBorrowalViewModel!.Customers = new(new Customer[] {newCustomer});
            NewBorrowalViewModel!.SelectedCustomer = newCustomer;

            NewBorrowalViewModel!.IsNewCustomer = true;

            CurrentViewModel = NewBorrowalViewModel;
        }

        private async Task LiftsChangeLiftStateAsync()
        {
            var selectedLift = LiftsViewModel.SelectedLift!;

            // Switch state
            if (selectedLift.Eliminated)
                selectedLift.Eliminated = false;
            else 
                selectedLift.Eliminated = true;

            try
            {
                ShowWaitingView();

                _mainUnitOfWork.LiftRepository.Update(selectedLift);
                await _mainUnitOfWork.SaveChangesAsync();

                CloseWaitingView();
                _logger.LogInformation($"Changed state of lift {selectedLift.SerialNumber} to {selectedLift.Eliminated}");
                await NotifyAboutSuccess("Úspěšně změněn stav plošiny.");
                await MenuNavigateToLiftsAsync();
                // TODO: Notify user about success
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private async Task LiftsShowNewLiftAsync()
        {
            EditLiftViewModel = _serviceProvider.GetRequiredService<EditLiftViewModel>();
            EditLiftViewModel.Caption = "Nová plošina";
            EditLiftViewModel.SerialNumberEnabled = true;
            EditLiftViewModel.CloseCommand = new AsyncRelayCommand(MenuNavigateToLiftsAsync);
            EditLiftViewModel.SubmitCommand = new AsyncRelayCommand(NewLiftSubmitAsync);

            try
            {
                ShowWaitingView();

                var offices = await _mainUnitOfWork.OfficeRepository.GetAsync(include:
                    offices => offices.Include(office => office.Address));

                EditLiftViewModel.Offices = new(offices);
                CurrentViewModel = EditLiftViewModel;

                CloseWaitingView();
                _logger.LogInformation("Navigated from Lifts to New Lift Form");
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private async Task LiftsShowEditLiftAsync()
        {
            EditLiftViewModel = _serviceProvider.GetRequiredService<EditLiftViewModel>();
            EditLiftViewModel.Caption = "Úprava záznamu plošiny";
            EditLiftViewModel.SerialNumberEnabled = false;
            EditLiftViewModel.CloseCommand = new AsyncRelayCommand(MenuNavigateToLiftsAsync);
            EditLiftViewModel.SubmitCommand = new AsyncRelayCommand(EditLiftSubmitAsync);
            EditLiftViewModel.LiftToEdit = LiftsViewModel.SelectedLift;

            try
            {
                ShowWaitingView();

                var offices = await _mainUnitOfWork.OfficeRepository.GetAsync(include:
                    offices => offices.Include(office => office.Address));

                EditLiftViewModel.Offices = new(offices);
                CurrentViewModel = EditLiftViewModel;

                CloseWaitingView();
                _logger.LogInformation("Navigated from Lifts to New Lift Form");
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private async Task NewLiftSubmitAsync()
        {
            EditLiftViewModel!.ValidateAll();
            if (EditLiftViewModel.HasErrors)
                return;

            try
            {
                ShowWaitingView();

                var newLift = new Lift()
                {
                    SerialNumber = EditLiftViewModel.SerialNumber!,
                    Manufacturer = EditLiftViewModel.Manufacturer!,
                    MaximumHeight = int.Parse(EditLiftViewModel.MaximumHeight!),
                    PowerSource = EditLiftViewModel.SelectedPowerSource!.Value,
                    Office = EditLiftViewModel.SelectedOffice!,
                    Eliminated = false
                };

                await _mainUnitOfWork.LiftRepository.InsertAsync(newLift);
                await _mainUnitOfWork.SaveChangesAsync();

                CloseWaitingView();
                _logger.LogInformation($"Inserted new lift with Serial number: {newLift.SerialNumber}");
                await NotifyAboutSuccess("Úspěšně vložena nová plošina.");
                await MenuNavigateToLiftsAsync();
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private async Task EditLiftSubmitAsync()
        {
            EditLiftViewModel!.ValidateAll();
            if (EditLiftViewModel.HasErrors)
                return;

            try
            {
                ShowWaitingView();

                EditLiftViewModel.LiftToEdit!.Manufacturer = EditLiftViewModel.Manufacturer!;
                EditLiftViewModel.LiftToEdit!.MaximumHeight = int.Parse(EditLiftViewModel.MaximumHeight!);
                EditLiftViewModel.LiftToEdit!.PowerSource = EditLiftViewModel.SelectedPowerSource!.Value;
                EditLiftViewModel.LiftToEdit!.Office = EditLiftViewModel.SelectedOffice!;

                _mainUnitOfWork.LiftRepository.Update(EditLiftViewModel.LiftToEdit!);
                await _mainUnitOfWork.SaveChangesAsync();

                CloseWaitingView();
                _logger.LogInformation($"Updated lift with Serial number: {EditLiftViewModel.LiftToEdit!.SerialNumber}");
                await NotifyAboutSuccess("Úspěšně upraven záznam plošiny.");
                await MenuNavigateToLiftsAsync();
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private async Task InvoicesShowNewInvoiceAsync()
        {
            try
            {
                ShowWaitingView();

                NewInvoiceViewModel = _serviceProvider.GetRequiredService<NewInvoiceViewModel>();
                NewInvoiceViewModel.CloseCommand = new AsyncRelayCommand(MenuNavigateToInvoicesAsync);
                NewInvoiceViewModel.SubmitCommand = new AsyncRelayCommand(NewInvoiceSubmitAsync);

                // Get available Invoice identifier
                var invoices = await _mainUnitOfWork.InvoiceRepository.GetAsync();
                var identifiers = invoices.Select(i => i.Identifier);
                var parsedIdentifiers = new List<(string, string)>();
                foreach(var identifier in identifiers)
                {
                    parsedIdentifiers.Add((identifier.Split('-')[0], identifier.Split('-')[1]));
                }
                var newIdentifierPostfix = 1;
                while (identifiers.Contains($"{DateTime.Now.Year}-{newIdentifierPostfix}"))
                {
                    newIdentifierPostfix++;
                }
                NewInvoiceViewModel.Identifier = $"{DateTime.Now.Year}-{newIdentifierPostfix}";

                // Fill Variable symbol
                NewInvoiceViewModel.VariableSymbol = $"{DateTime.Now.Year}{newIdentifierPostfix}";

                // Fill Borrowals ComboBox
                var borrowals = await _mainUnitOfWork.BorrowalRepository.GetAsync(include:
                    borrowals => borrowals.Include(borrowal => borrowal.TimeInterval),
                    orderBy: borrowals => borrowals
                        .OrderByDescending(borrowal => borrowal.TimeInterval.DateFrom));
                NewInvoiceViewModel.Borrowals = new(borrowals);

                CloseWaitingView();
                CurrentViewModel = NewInvoiceViewModel;
                _logger.LogInformation("Navigated from Invoices to New invoice form");
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private async Task NewInvoiceSubmitAsync()
        {
            NewInvoiceViewModel!.ValidateAll();
            if (NewInvoiceViewModel.HasErrors)
                return;

            var newInvoice = new Invoice()
            {
                Identifier = NewInvoiceViewModel.Identifier!,
                Borrowal = NewInvoiceViewModel.SelectedBorrowal!,
                DateOfIssue = DateOnly.FromDateTime(NewInvoiceViewModel.DateOfIssue!.Value),
                DueDate = DateOnly.FromDateTime(NewInvoiceViewModel.DueDate!.Value),
                DateOfTaxableSupply = DateOnly.FromDateTime(NewInvoiceViewModel.DateOfTaxableSupply!.Value),
                Price = int.Parse(NewInvoiceViewModel.Price!),
                ValueAddedTaxRate = float.Parse(NewInvoiceViewModel.ValueAddedTaxRate!),
                Bank = NewInvoiceViewModel.Bank!,
                BankAccount = NewInvoiceViewModel.BankAccount!,
                Description = NewInvoiceViewModel.Description,
                VariableSymbol = NewInvoiceViewModel.VariableSymbol!,
                IsExtra = NewInvoiceViewModel.IsExtra!.Value,
                Paid = false
            };

            try
            {
                ShowWaitingView();

                await _mainUnitOfWork.InvoiceRepository.InsertAsync(newInvoice);
                await _mainUnitOfWork.SaveChangesAsync();

                CloseWaitingView();
                _logger.LogInformation($"Inserted new Invoice with Identifier: {newInvoice.Identifier}");
                await NotifyAboutSuccess("Úspěšně vložena nová faktura.");
                await MenuNavigateToInvoicesAsync();
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private async Task InvoicesConfirmPaymentAsync()
        {
            var result = MessageBox.Show($"Opravdu chce potvrdit zaplacení faktury {InvoicesViewModel.SelectedInvoice!.Identifier}?",
                "Potvrdit zaplacení",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                ShowWaitingView();

                var invoiceToConmfirmPayment = InvoicesViewModel.SelectedInvoice!;
                InvoicesViewModel.SelectedInvoice = null;
                invoiceToConmfirmPayment.Paid = true;
                _mainUnitOfWork.InvoiceRepository.Update(invoiceToConmfirmPayment);
                await _mainUnitOfWork.SaveChangesAsync();

                CloseWaitingView();
                _logger.LogInformation($"Confirmed payment of Invoice: {invoiceToConmfirmPayment.Identifier}");
                await NotifyAboutSuccess("Úspěšně potvrzeno zaplacení faktury.");
                await MenuNavigateToInvoicesAsync();
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private async Task InvoicesDeleteInvoiceAsync()
        {
            var result = MessageBox.Show($"Opravdu chcete odstranit fakturu {InvoicesViewModel.SelectedInvoice!.Identifier}?",
                "Potvrdit zaplacení",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                ShowWaitingView();

                var invoiceToDelete = InvoicesViewModel.SelectedInvoice!;
                _mainUnitOfWork.InvoiceRepository.Delete(invoiceToDelete);
                await _mainUnitOfWork.SaveChangesAsync();

                CloseWaitingView();
                _logger.LogInformation($"Deleted invoice: {invoiceToDelete.Identifier}");
                await NotifyAboutSuccess("Úspěšně odstraněn záznam faktury.");
                await MenuNavigateToInvoicesAsync();
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private void InvoicesExportInvoice()
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.ShowNewFolderButton = true;
            var result = dialog.ShowDialog();
            if (result is null)
                return;
            if (!result.Value)
                return;

            try
            {
                ShowWaitingView();
                var invoiceToExport = InvoicesViewModel.SelectedInvoice!;
                var invoiceExportService = _serviceProvider.GetRequiredService<InvoiceExportService>();
                invoiceExportService.Export(dialog.SelectedPath, invoiceToExport);
                CloseWaitingView();
                Task.Run(() => ShowInfoBarAsync($"Faktura byla úspěšně exportována do adresáře: {dialog.SelectedPath}."));
                _logger.LogInformation($"Exported invoice file to destination: {dialog.SelectedPath}");
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private async Task MaintenancesDeleteMaintenanceAsync(Maintenance? maintenance)
        {
            if (maintenance == null)
                throw new ArgumentNullException(nameof(maintenance));

            var result = MessageBox.Show($"Opravdu chcete odstranit vybraný záznam o údržbě?",
                "Odstranění záznamu údržby",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                ShowWaitingView();

                _mainUnitOfWork.MaintenanceRepository.Delete(maintenance);
                await _mainUnitOfWork.SaveChangesAsync();

                CloseWaitingView();
                _logger.LogInformation($"Deleted maintenance: {maintenance.Id}");
                await NotifyAboutSuccess("Úspěšně odstraněn záznam o údržbě.");
                await MenuNavigateToMaintenancesAsync();
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private async Task MaintenancesShowEditMaintenanceAsync(Maintenance? maintenance)
        {
            EditMaintenanceViewModel = _serviceProvider.GetRequiredService<EditMaintenanceViewModel>();
            EditMaintenanceViewModel.CloseCommand = new AsyncRelayCommand(MenuNavigateToMaintenancesAsync);
            EditMaintenanceViewModel.SubmitCommand = new AsyncRelayCommand(EditMaintenanceSubmitEditedAsync);
            // New maintenance
            if (maintenance is null)
            {
                EditMaintenanceViewModel.Caption = "Nová údržba";

                try
                {
                    ShowWaitingView();

                    var lifts = await _mainUnitOfWork.LiftRepository.GetAsync(include:
                    lifts => lifts.Include(lift => lift.Office).ThenInclude(o => o.Address),
                    orderBy: lifts => lifts
                        .OrderBy(lift => lift.SerialNumber.Length)
                        .ThenBy(lift => lift.SerialNumber));
                    EditMaintenanceViewModel.Lifts = new(lifts);
                    CurrentViewModel = EditMaintenanceViewModel;

                    CloseWaitingView();
                    _logger.LogInformation($"Navigated From Maintenances to New Maintenance Form");
                }
                catch (Exception ex)
                {
                    CloseWaitingView();
                    _logger.LogError(ex, "");
                    ShowErrorWindow(ex.Message);
                }

            }
            // Edit existing maintenance
            else
            {
                EditMaintenanceViewModel.Caption = "Editace údržby";
                EditMaintenanceViewModel.MaintenanceToEdit = maintenance;
                CurrentViewModel = EditMaintenanceViewModel;
                _logger.LogInformation($"Navigated From Maintenances to New Maintenance Form");
            }
        }

        private async Task EditMaintenanceSubmitEditedAsync()
        {
            EditMaintenanceViewModel!.ValidateAll();
            if (EditMaintenanceViewModel.HasErrors)
                return;

            var dateFrom = DateOnly.FromDateTime(EditMaintenanceViewModel.DateFrom!.Value);
            var dateTo = DateOnly.FromDateTime(EditMaintenanceViewModel.DateTo!.Value);
            var maintenanceToEdit = EditMaintenanceViewModel.MaintenanceToEdit;

            var dateRangeValid = await MaintenanceDateRangeIsAvailableAsync(dateFrom, dateTo, maintenanceToEdit);
            if (!dateRangeValid)
            {
                MessageBox.Show("V zadaném časovém rozpětí má plošina již plánovanou výpůjčku nebo údržbu.",
                    "Nedostupné časové rozpětí",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            try
            {
                ShowWaitingView();

                // Add new maintenance
                if(maintenanceToEdit is null)
                {
                    var newMaintenance = new Maintenance()
                    {
                        Lift = EditMaintenanceViewModel.SelectedLift!,
                        TimeInterval = new TimeInterval()
                        {
                            DateFrom = dateFrom,
                            DateTo = dateTo
                        },
                        Description = EditMaintenanceViewModel.Description!,
                        Price = EditMaintenanceViewModel.Price is not null ? int.Parse(EditMaintenanceViewModel.Price) : null
                    };

                    await _mainUnitOfWork.MaintenanceRepository.InsertAsync(newMaintenance);
                    await _mainUnitOfWork.SaveChangesAsync();
                    _logger.LogInformation($"Inserted Maintenance with id: {newMaintenance.Id}");
                    await NotifyAboutSuccess("Úspěšně vložen záznam o údržbě.");
                }
                // Update existing maintenance
                else
                {
                    _mainUnitOfWork.TimeIntervalRepository.Delete(maintenanceToEdit.TimeInterval);
                    maintenanceToEdit.TimeInterval = new TimeInterval()
                    {
                        DateFrom = dateFrom,
                        DateTo = dateTo
                    };
                    maintenanceToEdit.Description = EditMaintenanceViewModel.Description!;
                    maintenanceToEdit.Price = EditMaintenanceViewModel.Price is not null ? int.Parse(EditMaintenanceViewModel.Price) : null;

                    _mainUnitOfWork.MaintenanceRepository.Update(maintenanceToEdit);
                    await _mainUnitOfWork.SaveChangesAsync();
                    _logger.LogInformation($"Updated Maintenance with id: {maintenanceToEdit.Id}");
                    await NotifyAboutSuccess("Úspěšně upraven záznam o údržbě.");
                }

                CloseWaitingView();
                await MenuNavigateToMaintenancesAsync();
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private async Task<bool> MaintenanceDateRangeIsAvailableAsync(DateOnly dateFrom, DateOnly dateTo, Maintenance? maintenanceToEdit = null)
        {
            var selectedLift = EditMaintenanceViewModel!.SelectedLift!;
            var selectedLiftBorrowals = await _mainUnitOfWork.BorrowalRepository.GetAsync(
                include: borrowals => borrowals
                .Include(borrowal => borrowal.Lift)
                .Include(borrowal => borrowal.TimeInterval),
                filter: borrowals => borrowals.Lift == selectedLift);

            var selectedLiftMaintenances = await _mainUnitOfWork.MaintenanceRepository.GetAsync(
                include: maintenances => maintenances
                .Include(maintenance => maintenance.Lift)
                .Include(maintenance => maintenance.TimeInterval),
                filter: maintenance => maintenance.Lift == selectedLift);

            if (maintenanceToEdit is not null)
                selectedLiftMaintenances = selectedLiftMaintenances.Except(new Maintenance[] { maintenanceToEdit });

            var mergedTimeIntervals = selectedLiftBorrowals.Select(b => b.TimeInterval).Union(selectedLiftMaintenances.Select(m => m.TimeInterval));

            return mergedTimeIntervals.All(
                timeInterval =>
                    (dateFrom < timeInterval.DateFrom && dateTo < timeInterval.DateFrom)
                    ||
                    (dateFrom > timeInterval.DateTo && dateTo > timeInterval.DateTo)
                );
        }

        private void MenuNavigateToExportAsync()
        {
            ExportViewModel = _serviceProvider.GetRequiredService<ExportViewModel>();
            ExportViewModel.SubmitCommand = new AsyncRelayCommand(ExportSubmitAsync);
            CurrentViewModel = ExportViewModel;
        }

        public async Task ExportSubmitAsync()
        {
            ExportViewModel!.ValidateAll();
            if (ExportViewModel.HasErrors)
                return;

            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog();
            dialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dialog.ShowNewFolderButton = true;
            var result = dialog.ShowDialog();
            if (result is null)
                return;
            if (!result.Value)
                return;

            var overviewConfigurations = new List<OverviewConfiguration>();
            if (ExportViewModel.BorrowalChecked)
            {
                if (ExportViewModel.BorrowalDateFrom is not null)
                    overviewConfigurations.Add(new(OverviewType.BorrowalOverview,
                        (DateOnly.FromDateTime(ExportViewModel.BorrowalDateFrom!.Value),
                        DateOnly.FromDateTime(ExportViewModel.BorrowalDateTo!.Value))));
                else
                    overviewConfigurations.Add(new(OverviewType.BorrowalOverview));
            }
            if (ExportViewModel.LiftChecked)
            {
                overviewConfigurations.Add(new(OverviewType.LiftOverview));
            }
            if (ExportViewModel.MaintenanceChecked)
            {
                if (ExportViewModel.MaintenanceDateFrom is not null)
                    overviewConfigurations.Add(new(OverviewType.MaintenanceOverview,
                        (DateOnly.FromDateTime(ExportViewModel.MaintenanceDateFrom!.Value),
                        DateOnly.FromDateTime(ExportViewModel.MaintenanceDateTo!.Value))));
                else
                    overviewConfigurations.Add(new(OverviewType.MaintenanceOverview));
            }
            if (ExportViewModel.InvoiceChecked)
            {
                if (ExportViewModel.InvoiceDateFrom is not null)
                    overviewConfigurations.Add(new(OverviewType.InvoiceOverview,
                        (DateOnly.FromDateTime(ExportViewModel.InvoiceDateFrom!.Value),
                        DateOnly.FromDateTime(ExportViewModel.InvoiceDateTo!.Value))));
                else
                    overviewConfigurations.Add(new(OverviewType.InvoiceOverview));
            }
            if (ExportViewModel.CustomerChecked)
            {
                overviewConfigurations.Add(new(OverviewType.CustomerOverview));
            }

            try
            {
                ShowWaitingView();
                var exportService = _serviceProvider.GetRequiredService<OverviewExportService>();
                await exportService.ExportAsync(dialog.SelectedPath, overviewConfigurations);
                CloseWaitingView();
                await NotifyAboutSuccess($"Úspěšně vyexportován přehled dat do adresáře: {dialog.SelectedPath}");
                _logger.LogInformation($"Exported overviews file to destination: {dialog.SelectedPath}");
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        } 

        private async Task MenuNavigateToStatisticsAsync()
        {
            StatisticsViewModel = _serviceProvider.GetRequiredService<StatisticsViewModel>();
            StatisticsViewModel.OverallIncomeByDaysViewModel = _serviceProvider.GetRequiredService<OverallIncomeByDaysViewModel>();
            StatisticsViewModel.IncomeByCustomerViewModel = _serviceProvider.GetRequiredService<IncomeByCustomerViewModel>();
            StatisticsViewModel.BorrowedLiftTypeByDaysViewModel = _serviceProvider.GetRequiredService<BorrowedLiftTypeByDaysViewModel>();
            StatisticsViewModel.IncomeYearComparisonViewModel = _serviceProvider.GetRequiredService<IncomeYearComparisonViewModel>();

            try
            {
                ShowWaitingView();
                var borrowals = await _mainUnitOfWork.BorrowalRepository.GetAsync(include: borrowals => borrowals
                .Include(borrowal => borrowal.TimeInterval)
                .Include(borrowal => borrowal.Lift).ThenInclude(lift => lift.Office).ThenInclude(office => office.Address));

                // Overall income by days
                var overallIncomeByDaysService = _serviceProvider.GetRequiredService<BorrowalReportServiceFactory>().Create(BorrowalReportType.OverallIncomeByDays);
                (StatisticsViewModel.OverallIncomeByDaysViewModel.Series,
                    StatisticsViewModel.OverallIncomeByDaysViewModel.XAxes) = overallIncomeByDaysService.GetSeriesAndAxes(borrowals);

                // Income by customer
                var incomeByCustomerService = _serviceProvider.GetRequiredService<BorrowalReportServiceFactory>().Create(BorrowalReportType.IncomeByCustomers);
                (StatisticsViewModel.IncomeByCustomerViewModel.Series,
                    StatisticsViewModel.IncomeByCustomerViewModel.XAxes) = incomeByCustomerService.GetSeriesAndAxes(borrowals);
                StatisticsViewModel.IncomeByCustomerViewModel.Customers = new(borrowals
                    .Select(b => b.Customer)
                    .Distinct()
                    .OrderBy(c => c.Identifier.Length)
                    .ThenBy(c => c.Identifier));

                // Borrowed lift type by days
                var borrowedLiftTypeByDaysReportService = _serviceProvider.GetRequiredService<BorrowalReportServiceFactory>().Create(BorrowalReportType.BorrowedLiftTypeByDays);
                (StatisticsViewModel.BorrowedLiftTypeByDaysViewModel.Series,
                    StatisticsViewModel.BorrowedLiftTypeByDaysViewModel.XAxes) = borrowedLiftTypeByDaysReportService.GetSeriesAndAxes(borrowals);

                // Income year comparison
                StatisticsViewModel.IncomeYearComparisonViewModel.UpdateReportCommand = new RelayCommand(() => IncomeYearComparisonUpdateReport(borrowals));
                StatisticsViewModel.IncomeYearComparisonViewModel.Years = new(borrowals
                    .Select(b => b.TimeInterval.DateFrom)
                    .Union(borrowals.Select(b => b.TimeInterval.DateTo))
                    .Select(ti => ti.Year)
                    .Distinct()
                    .OrderBy(year => year));


                StatisticsViewModel.SelectedBorrowalReportType = StatisticsViewModel.BorrowalReportTypes.First();
                CurrentViewModel = StatisticsViewModel;
                CloseWaitingView();
                _logger.LogInformation("Navigated to Statistics");
            }
            catch (Exception ex)
            {
                CloseWaitingView();
                _logger.LogError(ex, "");
                ShowErrorWindow(ex.Message);
            }
        }

        private void IncomeYearComparisonUpdateReport(IEnumerable<Borrowal> borrowals)
        {
            StatisticsViewModel!.IncomeYearComparisonViewModel!.ValidateAll();
            if (StatisticsViewModel.IncomeYearComparisonViewModel.HasErrors)
            {
                StatisticsViewModel.IncomeYearComparisonViewModel.Series = null;
                StatisticsViewModel.IncomeYearComparisonViewModel.XAxes = null;
                return;
            }
            var incomeYearComparisonReportService = _serviceProvider.GetRequiredService<BorrowalReportServiceFactory>().Create(BorrowalReportType.IncomeYearComparison);
            var firstYear = StatisticsViewModel!.IncomeYearComparisonViewModel!.SelectedFirstYear;
            var secondYear = StatisticsViewModel!.IncomeYearComparisonViewModel!.SelectedSecondYear;
            (StatisticsViewModel.IncomeYearComparisonViewModel.Series,
                StatisticsViewModel.IncomeYearComparisonViewModel.XAxes) = incomeYearComparisonReportService
                .GetSeriesAndAxes(borrowals, (firstYear!.Value, secondYear!.Value));
        }

        private void ShowWaitingView()
        {
            _waitingView = _serviceProvider.GetRequiredService<WaitingView>();
            _waitingView.Owner = _serviceProvider.GetRequiredService<MainWindow>();
            _waitingView.Show();
        }

        private void CloseWaitingView()
        {
            if (_waitingView == null)
                throw new NullReferenceException("Waiting view is null");
            _waitingView.Close();
        }

        private void ShowErrorWindow(string message)
        {
            var errorWindow = _serviceProvider.GetRequiredService<ErrorWindow>();
            errorWindow.Owner = _serviceProvider.GetRequiredService<MainWindow>();
            errorWindow.ErrorMessage.Text = message;
            errorWindow.ShowDialog();
        }

        private async Task NotifyAboutSuccess(string message)
        {
            await Task.Factory.StartNew(() => ShowInfoBarAsync(message));
        }

        private async Task ShowInfoBarAsync(string message)
        {
            InfoBarViewModel.Text = message;
            InfoBarVisible = true;
            await Task.Delay(2000);
            InfoBarVisible = false;
        }
    }
}
