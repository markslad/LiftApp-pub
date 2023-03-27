using LiftApp.Export.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Export.Models
{
    public class OverviewConfiguration
    {
        public OverviewType OverviewType { get; private set; }
        public (DateOnly dateFrom, DateOnly dateTo)? DateRange { get; private set; }

        public OverviewConfiguration(OverviewType overviewType, (DateOnly dateFrom, DateOnly dateTo)? dateRange = null)
        {
            OverviewType = overviewType;
            DateRange = dateRange;
        }
    }
}
