using LiftApp.Dal.Models;
using LiftApp.Statistics.Interfaces;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Statistics.Services
{
    public class IncomeYearComparisonReportService : IBorowalReportService
    {
        public (ISeries[] series, Axis[] xAxes) GetSeriesAndAxes(IEnumerable<Borrowal> borrowals, (int firstYear, int secondYear)? yearRange = null)
        {
            if (yearRange is null)
                throw new ArgumentNullException(nameof(yearRange));
            if (yearRange.Value.secondYear <= yearRange.Value.firstYear)
                throw new ArgumentException($"Invalid year range", nameof(yearRange));

            var startDate = new DateTime(yearRange.Value.firstYear, 1, 1);
            var endDate = new DateTime(yearRange.Value.secondYear, 12, 31);
            var allDays = Enumerable.Range(0, 1 + endDate.Subtract(startDate).Days)
                .Select(offset => startDate.AddDays(offset))
                .Select(day => DateOnly.FromDateTime(day));

            var sums = new Dictionary<(int Year, int Month), int>();
            foreach(var day in allDays)
            {
                if(!sums.ContainsKey((day.Year, day.Month)))
                    sums.Add((day.Year, day.Month), 0);

                var incomeInDay = borrowals
                    .Where(b => b.TimeInterval.DateFrom <= day && b.TimeInterval.DateTo >= day)
                    .Sum(b => b.PriceADay);
                sums[(day.Year, day.Month)] += incomeInDay;
            }

            var firstYearMonthSums = sums
                .Where(item => item.Key.Year == yearRange.Value.firstYear)
                .Select(item => item.Value);
            var secondYearMonthSums = sums
                .Where(item => item.Key.Year == yearRange.Value.secondYear)
                .Select(item => item.Value);

            return (new ColumnSeries<int>[]
            {
                new ColumnSeries<int>
                {
                    Name = yearRange.Value.firstYear.ToString(),
                    Values = firstYearMonthSums,
                    TooltipLabelFormatter =
                        (chartPoint) => $"{chartPoint.Context.Series.Name}: {chartPoint.PrimaryValue},-"
                },
                new ColumnSeries<int>
                {
                    Name = yearRange.Value.secondYear.ToString(),
                    Values = secondYearMonthSums,
                    TooltipLabelFormatter =
                        (chartPoint) => $"{chartPoint.Context.Series.Name}: {chartPoint.PrimaryValue},-"
                }
            },
            new Axis[]
            {
                new Axis()
                {
                    Labels = new string[]
                    {
                        "Leden",
                        "Únor",
                        "Březen",
                        "Duben",
                        "Květen",
                        "Červen",
                        "Červenec",
                        "Srpen",
                        "Září",
                        "Říjen",
                        "Listopad",
                        "Prosinec"
                    },
                    LabelsRotation = 0,
                    SeparatorsPaint = new SolidColorPaint(new SKColor(200, 200, 200)),
                    SeparatorsAtCenter = false,
                }
            });
        }
    }
}
