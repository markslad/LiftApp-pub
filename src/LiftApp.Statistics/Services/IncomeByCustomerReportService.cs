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
    public class IncomeByCustomerReportService : IBorowalReportService
    {
        public (ISeries[] series, Axis[] xAxes) GetSeriesAndAxes(IEnumerable<Borrowal> borrowals, (int firstYear, int secondYear)? yearRange = null)
        {
            if (yearRange is not null)
                throw new NotSupportedException($"Year range for {nameof(IncomeByCustomerReportService)} is not supported.");

            var customers = borrowals.Select(b => b.Customer).Distinct();

            var series = new List<StackedColumnSeries<DateTimePoint>>();
            foreach (var customer in customers)
            {
                var filteredBorrowals = borrowals.Where(b => b.Customer == customer);

                var allDatesDistinct = filteredBorrowals.Select(borrowal => borrowal.TimeInterval.DateFrom)
                .Union(filteredBorrowals.Select(borrowal => borrowal.TimeInterval.DateTo))
                .Distinct();
                var firstDate = allDatesDistinct.Min();
                var lastDate = allDatesDistinct.Max();

                var points = new List<DateTimePoint>();
                for (DateOnly date = firstDate; date.CompareTo(lastDate) <= 0; date = date.AddDays(1))
                {
                    var incomeInDay = filteredBorrowals.Where(b => b.TimeInterval.DateFrom <= date && b.TimeInterval.DateTo >= date).Sum(b => b.PriceADay);
                    points.Add(new DateTimePoint(date.ToDateTime(TimeOnly.MinValue), incomeInDay));
                }
                series.Add(new StackedColumnSeries<DateTimePoint>()
                {
                    Values = points,
                    Name = customer.Identifier,
                    TooltipLabelFormatter =
                        (chartPoint) => $"{chartPoint.Model!.DateTime.ToShortDateString()}: {chartPoint.PrimaryValue},-"
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
