using LiftApp.Dal.Models;
using LiftApp.Statistics.Interfaces;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Statistics.Services
{
    public class BorrowedLiftTypeByDaysReportService : IBorowalReportService
    {
        public (ISeries[] series, Axis[] xAxes) GetSeriesAndAxes(IEnumerable<Borrowal> borrowals, (int firstYear, int secondYear)? yearRange = null)
        {
            if (yearRange is not null)
                throw new NotSupportedException($"Year range for {nameof(BorrowedLiftTypeByDaysReportService)} is not supported.");

            var allDatesDistinct = borrowals.Select(borrowal => borrowal.TimeInterval.DateFrom)
                .Union(borrowals.Select(borrowal => borrowal.TimeInterval.DateTo))
                .Distinct();
            var firstDate = allDatesDistinct.Min();
            var lastDate = allDatesDistinct.Max();

            var borrowedLiftTypes = borrowals.Select(b => b.Lift)
                .Select(l => l.PowerSource)
                .Distinct();

            var series = new List<StackedColumnSeries<DateTimePoint>>();
            foreach(var liftType in borrowedLiftTypes)
            {
                var points = new List<DateTimePoint>();
                for (DateOnly date = firstDate; date.CompareTo(lastDate) <= 0; date = date.AddDays(1))
                {
                    var liftsCount = borrowals.Where(b => b.TimeInterval.DateFrom <= date && b.TimeInterval.DateTo >= date)
                        .Where(b => b.Lift.PowerSource == liftType)
                        .Count();
                    points.Add(new DateTimePoint(date.ToDateTime(TimeOnly.MinValue), liftsCount));
                }
                series.Add(new StackedColumnSeries<DateTimePoint>()
                {
                    Values = points,
                    Name = liftType.ToString(),
                    TooltipLabelFormatter =
                        (chartPoint) => $"{chartPoint.Model!.DateTime.ToShortDateString()}: " +
                        $"{chartPoint.Context.Series.Name}: " +
                        $"{chartPoint.PrimaryValue} ks"
                });
            }

            return (series.ToArray(),
                new Axis[]
                {
                    new Axis()
                    {
                        Labeler = value => new DateTime((long)value).ToString("dd. MM. yyyy"),
                        LabelsRotation = 75,
                        UnitWidth = TimeSpan.FromDays(1).Ticks,
                        MinStep = TimeSpan.FromDays(1).Ticks
                    }
                });
        }
    }
}
