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
                    //DefaultSeriesType = ChartDefaults.OverallClientCountChartType,
                    DefaultSeriesType = ChartTypes.Areaspline,
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
                    Areaspline = new PlotOptionsAreaspline()
                    //Spline = new PlotOptionsSpline()
                    {
                        //Marker = new PlotOptionsSplineMarker()
                        Marker = new PlotOptionsAreasplineMarker()
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

        public async Task<Highcharts> GetPerVlanClientCount(int lastXHours)
        {
            var data = await _database.GetPerVlanClientCountLastXHours(lastXHours);

            var vlanSeriesList = new List<Series>();

            foreach (var vlanGroup in data.OrderBy(d => d.Vlan).ThenBy(ap => ap.BatchDate).GroupBy(ap => ap.Vlan))
            {
                vlanSeriesList.Add(new Series
                {
                    Name = vlanGroup.Key,
                    Data = new Data(vlanGroup.Select(g => new object[] { g.BatchDate, g.Clients }).ToArray()),
                    //PlotOptionsSpline = visibleAps.Contains(apGroup.Key)
                    //? new PlotOptionsSpline() { Visible = true }
                    //: new PlotOptionsSpline() { Visible = false }
                });
            }

            var chart = new Highcharts($"perVlanClientCount{lastXHours}hours")
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
                    Text = $"Clients per VLAN - last {lastXHours} {(lastXHours == 1 ? "hour" : "hours")}"
                })
                .SetCredits(new Credits { Enabled = false })
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
                .SetSeries(vlanSeriesList.ToArray());

            return chart;
        }

        public async Task<Highcharts> GetPerAccessPointClientCount(int lastXHours, int showTopX = 5)
        {
            var apSeriesList = new List<Series>();
            var data = await _database.GetPerAccessClientCountLastXHours(lastXHours);

            var visibleAps = new List<string>();
            var apMax = new List<Tuple<string, long>>();
            // Get the access points that had the 5 highest client counts at any point
            foreach (var apGroup in data.GroupBy(ap => ap.AccessPoint))
            {
                // Get the single max for this group
                var groupMax = apGroup.Max(ap => ap.Clients);
                apMax.Add(new Tuple<string, long>(apGroup.Key, groupMax));
            }
            foreach (var apClientCountValue in apMax.OrderByDescending(ap => ap.Item2).TakeWhile(c => visibleAps.Count < 5))
            {
                if (!visibleAps.Contains(apClientCountValue.Item1))
                {
                    visibleAps.Add(apClientCountValue.Item1);
                }
            }

            foreach (var apGroup in data.OrderBy(ap => ap.AccessPoint).ThenBy(ap => ap.BatchDate).GroupBy(ap => ap.AccessPoint))
            {
                apSeriesList.Add(new Series
                {
                    Name = apGroup.Key,
                    Data = new Data(data.Select(g => new object[] { g.BatchDate, g.Clients }).ToArray()),
                    PlotOptionsSpline = visibleAps.Contains(apGroup.Key)
                    ? new PlotOptionsSpline() { Visible = true }
                    : new PlotOptionsSpline() { Visible = false }
                });
            }

            //var vlanSeriesList = new List<Series>();

            //foreach (var vlanGroup in data.OrderBy(d => d.Vlan).ThenBy(ap => ap.BatchDate).GroupBy(ap => ap.Vlan))
            //{
            //    vlanSeriesList.Add(new Series
            //    {
            //        Name = vlanGroup.Key,
            //        Data = new Data(vlanGroup.Select(g => new object[] { g.BatchDate, g.Clients }).ToArray()),
            //        //PlotOptionsSpline = visibleAps.Contains(apGroup.Key)
            //        //? new PlotOptionsSpline() { Visible = true }
            //        //: new PlotOptionsSpline() { Visible = false }
            //    });
            //}

            var chart = new Highcharts($"perAccessPointClientCount{lastXHours}hours")
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
                    Text = $"Client count per AP - last {lastXHours} {(lastXHours == 1 ? "hour" : "hours")}"
                })
                .SetCredits(new Credits { Enabled = false })
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
                .SetSeries(apSeriesList.ToArray());

            return chart;
        }

        public async Task<Highcharts> GetClientsPerMonth(int monthNumber, int apId = 0, int vlanId = 0)
        {

            var startDate = new DateTime(DateTime.Today.Year, monthNumber, 1);
            var maxDay = DateTime.DaysInMonth(DateTime.Today.Year, monthNumber);
            var endDate = new DateTime(DateTime.Today.Year, monthNumber, maxDay, 23, 59, 59);

            var data = await _database.GetClientCountSummary(startDate, endDate, apId, vlanId);
            var vlanName = "";
            if (vlanId > 0)
            {
                vlanName  = (await _database.GetVlanAsync(vlanId)).Value;
            }
            var accessPointName = "";
            if (apId > 0)
            {
                accessPointName = (await _database.GetAccessPointAsync(apId)).Name;
            }

            var title = $"Overall client count for {startDate:MMMM}";
            if (apId > 0)
            {
                title = $"Overall client count for {startDate:MMMM} and AP {accessPointName}";
            }
            else if (vlanId > 0)
            {
                title = $"Overall client count for {startDate:MMMM} and VLAN {vlanName}";
            }

            var chart = new Highcharts($"ClientCountMonth{monthNumber}ap{apId}vlan{vlanId}")
                .InitChart(new Chart
                {
                    //DefaultSeriesType = ChartDefaults.OverallClientCountChartType,
                    DefaultSeriesType = ChartTypes.Areaspline,
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
                    Text = title
                })
                .SetCredits(new Credits { Enabled = false })
                .SetPlotOptions(new PlotOptions
                {
                    Areaspline = new PlotOptionsAreaspline()
                    //Spline = new PlotOptionsSpline()
                    {
                        //Marker = new PlotOptionsSplineMarker()
                        Marker = new PlotOptionsAreasplineMarker()
                        {
                            Enabled = false
                        },
                    }
                })
                .SetOptions(new GlobalOptions() { Global = new Global { UseUTC = false } })
                .SetSeries(new Series() { Name = "Client count", Data = new Data(data.Select(c => new object[] { c.Date, c.ClientCount }).ToArray()) });
            
            return chart;
        }
    }
}