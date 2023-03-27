using LiftApp.Dal.Models;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Statistics.Interfaces
{
    public interface IBorowalReportService
    {
        public (ISeries[] series, Axis[] xAxes) GetSeriesAndAxes(IEnumerable<Borrowal> borrowals, (int firstYear, int secondYear)? yearRange = null);
    }
}
