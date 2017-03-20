using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Database;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;

namespace WebMvc5.Helpers.Charts
{
    public class ChartRepository : IChartRepository
    {
        private readonly IDatabaseRepository _database;

        public ChartRepository(IDatabaseRepository database)
        {
            _database = database;
        }

        public async Task<Highcharts> GetOverallClientCount(int lastXHours)
        {
            var data = await _database.GetOverallClientCountLastXHours(lastXHours);

            var chart = new Highcharts($"overallClientCount{lastXHours}hours")
                .InitChart(new Chart
                {
                    DefaultSeriesType = ChartDefaults.OverallClientCountChartType,
                    ZoomType = ChartDefaults.OverallClientCountZoomType,
                    Height = ChartDefaults.OverallClientCountChartHeight,
                })
                .SetXAxis(new XAxis
                {
                    Type = AxisTypes.Datetime,
                    DateTimeLabelFormats = new DateTimeLabel
                    {
                        Month = ChartDefaults.OverallClientCountDateTimeLabelFormatMonth,
                        Year = ChartDefaults.OverallClientCountDateTimeLabelFormatYear,
                        Day = ChartDefaults.OverallClientCountDateTimeLabelFormatDay,
                        Week = ChartDefaults.OverallClientCountDateTimeLabelFormatWeek,
                        Hour = ChartDefaults.OverallClientCountDateTimeLabelFormatHour,
                        Minute = ChartDefaults.OverallClientCountDateTimeLabelFormatMinute
                    }
                })
                .SetYAxis(new YAxis
                {
                    Title = new YAxisTitle { Text = "Clients" },
                    Min = 0
                })
                .SetTitle(new Title
                {
                    Text = $"Overall client count - last {lastXHours} {(lastXHours == 1 ? "hour" : "hours")}"
                })
                .SetCredits(new Credits{Enabled = false})
                .SetPlotOptions(new PlotOptions
                {
                    Spline = new PlotOptionsSpline()
                    {
                        Marker = new PlotOptionsSplineMarker()
                        {
                            Enabled = false
                        },
                    }
                })
                .SetOptions(new GlobalOptions() { Global = new Global { UseUTC = false } })
                .SetSeries(new Series() { Name = "Client count", Data = new Data(data.Select(c => new object[] { c.BatchDate, c.Clients }).ToArray()) });

            return chart;
        }

        public async Task<Highcharts> GetOverallClientCount(ChartPeriod chartPeriod)
        {
            throw new NotImplementedException();
        }
    }
}