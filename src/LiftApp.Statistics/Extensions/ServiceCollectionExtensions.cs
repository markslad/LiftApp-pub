using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftApp.Statistics.Services;
using LiftApp.Statistics.Factories;

namespace LiftApp.Statistics.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterReportServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<BorrowalReportServiceFactory>();
            services.AddTransient<OverallIncomeByDaysReportService>();
            services.AddTransient<IncomeByCustomerReportService>();
            services.AddTransient<BorrowedLiftTypeByDaysReportService>();
            services.AddTransient<IncomeYearComparisonReportService>();
        }
    }
}
