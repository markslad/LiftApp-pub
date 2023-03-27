using LiftApp.Dal.Models;
using LiftApp.Statistics.Interfaces;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftApp.Statistics.Services
{
    public class OverallIncomeByDaysReportService : IBorowalReportService
    {
        public (ISeries[] series, Axis[] xAxes) GetSeriesAndAxes(IEnumerable<Borrowal> borrowals, (int firstYear, int secondYear)? yearRange = null)
        {
            if (yearRange is not null)
                throw new NotSupportedException($"Year range for {nameof(OverallIncomeByDaysReportService)} is not supported.");

            if (borrowals.Select(b => b.TimeInterval).Any(ti => ti == null))
                throw new NullReferenceException("Any of Borrowal Time interval is null");

            var allDatesDistinct = borrowals.Select(borrowal => borrowal.TimeInterval.DateFrom)
                .Union(borrowals.Select(borrowal => borrowal.TimeInterval.DateTo))
                .Distinct();
            var firstDate = allDatesDistinct.Min();
            var lastDate = allDatesDistinct.Max();

            var points = new List<DateTimePoint>();
            for(DateOnly date = firstDate; date.CompareTo(lastDate) <= 0; date = date.AddDays(1))
            {
                var incomeInDay = borrowals.Where(b => b.TimeInterval.DateFrom <= date && b.TimeInterval.DateTo >= date).Sum(b => b.PriceADay);
                points.Add(new DateTimePoint(date.ToDateTime(TimeOnly.MinValue), incomeInDay));
            }

            return (new LineSeries<DateTimePoint>[]
            {
                new LineSeries<DateTimePoint>()
                {
                    Values = points,
                    Fill = new SolidColorPaint(SKColors.CornflowerBlue),
                    GeometrySize = 0,
                    LineSmoothness = 0,
                    Stroke = null,
                    GeometryFill = null,
                    GeometryStroke = null,
                    TooltipLabelFormatter =
                        (chartPoint) => $"{chartPoint.Model!.DateTime.ToShortDateString()}: {chartPoint.PrimaryValue},-"
                }
            },
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
