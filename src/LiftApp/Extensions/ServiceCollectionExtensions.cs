using LiftApp.Dal.Contexts;
using LiftApp.Dal.Interfaces;
using LiftApp.Dal.UnitsOfWork;
using LiftApp.ViewModels;
using LiftApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterViews(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<MainWindow>();
            services.AddTransient<ErrorWindow>();
            services.AddTransient<BorrowalsUserControl>();
            services.AddTransient<LiftDetailUserControl>();
            services.AddTransient<CustomerDetailUserControl>();
            services.AddTransient<LiftsUserControl>();
            services.AddTransient<CustomersUserControl>();
            services.AddTransient<InvoicesUserControl>();
            services.AddTransient<WaitingView>();
            services.AddTransient<BorrowalDetailUserControl>();
            services.AddTransient<MaintenancesUserControl>();
            services.AddTransient<NewBorrowalUserControl>();
            services.AddTransient<NewCustomerUserControl>();
            services.AddTransient<EditLiftUserControl>();
            services.AddTransient<NewInvoiceUserControl>();
            services.AddTransient<EditMaintenanceUserControl>();
            services.AddTransient<ExportUserControl>();
            services.AddTransient<InfoBarUserControl>();
            services.AddTransient<StatisticsUserControl>();
            services.AddTransient<OverallIncomeByDaysUserControl>();
            services.AddTransient<IncomeByCustomerUserControl>();
            services.AddTransient<BorrowedLiftTypeByDaysUserControl>();
            services.AddTransient<IncomeYearComparisonUserControl>();
        }

        public static void RegisterViewModels(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<BorrowalsViewModel>();
            services.AddTransient<LiftDetailViewModel>();
            services.AddTransient<CustomerDetailViewModel>();
            services.AddTransient<LiftsViewModel>();
            services.AddTransient<CustomersViewModel>();
            services.AddTransient<InvoicesViewModel>();
            services.AddTransient<BorrowalDetailViewModel>();
            services.AddTransient<MaintenancesViewModel>();
            services.AddTransient<NewBorrowalViewModel>();
            services.AddTransient<NewCustomerViewModel>();
            services.AddTransient<EditLiftViewModel>();
            services.AddTransient<NewInvoiceViewModel>();
            services.AddTransient<EditMaintenanceViewModel>();
            services.AddTransient<ExportViewModel>();
            services.AddTransient<InfoBarViewModel>();
            services.AddTransient<StatisticsViewModel>();
            services.AddTransient<OverallIncomeByDaysViewModel>();
            services.AddTransient<IncomeByCustomerViewModel>();
            services.AddTransient<BorrowedLiftTypeByDaysViewModel>();
            services.AddTransient<IncomeYearComparisonViewModel>();
        }
    }
}
