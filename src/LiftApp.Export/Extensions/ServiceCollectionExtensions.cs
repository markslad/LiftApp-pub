using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftApp.Export.Options;
using LiftApp.Export.Renderers;
using LiftApp.Export.Interfaces;
using LiftApp.Export.Services;
using LiftApp.Export.Factories;

namespace LiftApp.Export.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterExportServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<OverviewExportService>();
            services.AddTransient<OverviewSheetRendererFactory>();
            services.AddTransient<BorrowalOverviewSheetRenderer>();
            services.AddTransient<LiftOverviewSheetRenderer>();
            services.AddTransient<MaintenanceOverviewSheetRenderer>();
            services.AddTransient<InvoiceOverviewSheetRenderer>();
            services.AddTransient<CustomerOverviewSheetRenderer>();

            services.AddTransient<InvoiceExportService>();

            services.Configure<OverviewExportOptions>(configuration.GetSection(nameof(OverviewExportOptions)));
            services.Configure<BorrowalOverviewOptions>(configuration.GetSection(nameof(BorrowalOverviewOptions)));
            services.Configure<LiftOverviewOptions>(configuration.GetSection(nameof(LiftOverviewOptions)));
            services.Configure<MaintenanceOverviewOptions>(configuration.GetSection(nameof(MaintenanceOverviewOptions)));
            services.Configure<InvoiceOverviewOptions>(configuration.GetSection(nameof(InvoiceOverviewOptions)));
            services.Configure<CustomerOverviewOptions>(configuration.GetSection(nameof(CustomerOverviewOptions)));

            services.Configure<InvoiceExportOptions>(configuration.GetSection(nameof(InvoiceExportOptions)));
            services.Configure<CompanyOptions>(configuration.GetSection(nameof(CompanyOptions)));
        }
    }
}
