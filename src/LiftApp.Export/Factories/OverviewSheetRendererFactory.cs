using LiftApp.Export.Enums;
using LiftApp.Export.Interfaces;
using LiftApp.Export.Renderers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Factories
{
    public class OverviewSheetRendererFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public OverviewSheetRendererFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IOverviewSheetRenderer Create(OverviewType type)
        {
            return type switch
            {
                OverviewType.BorrowalOverview => _serviceProvider.GetRequiredService<BorrowalOverviewSheetRenderer>(),
                OverviewType.LiftOverview => _serviceProvider.GetRequiredService<LiftOverviewSheetRenderer>(),
                OverviewType.MaintenanceOverview => _serviceProvider.GetRequiredService<MaintenanceOverviewSheetRenderer>(),
                OverviewType.InvoiceOverview => _serviceProvider.GetRequiredService<InvoiceOverviewSheetRenderer>(),
                OverviewType.CustomerOverview => _serviceProvider.GetRequiredService<CustomerOverviewSheetRenderer>(),
                _ => throw new NotImplementedException("Unkown OverviewSheetRendererType")
            };
        }
    }
}
