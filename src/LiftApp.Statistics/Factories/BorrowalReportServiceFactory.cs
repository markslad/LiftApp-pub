using LiftApp.Statistics.Enums;
using LiftApp.Statistics.Interfaces;
using LiftApp.Statistics.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Statistics.Factories
{
    public class BorrowalReportServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public BorrowalReportServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IBorowalReportService Create(BorrowalReportType borrowalReportType)
        {
            return borrowalReportType switch
            {
                BorrowalReportType.OverallIncomeByDays => _serviceProvider.GetRequiredService<OverallIncomeByDaysReportService>(),
                BorrowalReportType.IncomeByCustomers => _serviceProvider.GetRequiredService<IncomeByCustomerReportService>(),
                BorrowalReportType.BorrowedLiftTypeByDays => _serviceProvider.GetRequiredService<BorrowedLiftTypeByDaysReportService>(),
                BorrowalReportType.IncomeYearComparison => _serviceProvider.GetRequiredService<IncomeYearComparisonReportService>(),
                _ => throw new NotImplementedException("Unkonwn Income report type")
            };
        }
    }
}
