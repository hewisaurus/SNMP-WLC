using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Database;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using WebMvc5.Helpers.Charts;
using WebMvc5.Models.Home;

namespace WebMvc5.Controllers
{
    public class HomeController : Controller
    {
        private readonly IChartRepository _chart;
        private readonly IDatabaseRepository _database;

        public HomeController(IDatabaseRepository database, IChartRepository chart)
        {
            _database = database;
            _chart = chart;
        }

        public async Task<ActionResult> Index()
        {
            var model = new IndexViewModel
            {
                OverallClientCountLastHour = await _chart.GetOverallClientCount(1),
                OverallClientCountLast2Hours = await _chart.GetOverallClientCount(2),
                OverallClientCountLast4Hours = await _chart.GetOverallClientCount(4),
                OverallClientCountLast8Hours = await _chart.GetOverallClientCount(8),
                OverallClientCountLast16Hours = await _chart.GetOverallClientCount(16),
                OverallClientCountLast24Hours = await _chart.GetOverallClientCount(24),
                OverallClientCountLast48Hours = await _chart.GetOverallClientCount(48)
            };

            return View(model);
        }
        public async Task<ActionResult> Index2()
        {
            var model = new IndexViewModel();

            var apClientCount = await _database.GetAccessPointClientCountToday();
            var apSeriesList = new List<Series>();
            var visibleAps = new List<string>();
            var apMax = new List<Tuple<string, long>>();
            // Get the access points that had the 5 highest client counts at any point
            foreach (var apGroup in apClientCount.GroupBy(ap => ap.AccessPoint))
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

            foreach (var apGroup in apClientCount.OrderBy(ap => ap.AccessPoint).ThenBy(ap => ap.BatchDate).GroupBy(ap => ap.AccessPoint))
            {
                apSeriesList.Add(new Series
                {
                    Name = apGroup.Key,
                    Data = new Data(apGroup.Select(g => new object[] { g.BatchDate, g.Clients }).ToArray()),
                    PlotOptionsSpline = visibleAps.Contains(apGroup.Key)
                    ? new PlotOptionsSpline() { Visible = true }
                    : new PlotOptionsSpline() { Visible = false }
            });
            }

            model.AccessPointClientCount = new Highcharts("AccessPointClientCount")
                .InitChart(new Chart()
                {
                    DefaultSeriesType = ChartTypes.Spline,
                    ZoomType = ZoomTypes.Xy,
                    Height = 800
                })
                .SetXAxis(new XAxis
                {
                    Type = AxisTypes.Datetime,
                    DateTimeLabelFormats = new DateTimeLabel
                    {
                        Month = "%e %b",
                        Year = "%e %b",
                        Day = "%e %b",
                        Week = "%e %b",
                        Hour = "%I %p",
                        Minute = "%I:%M %p"
                    }

                })
                .SetYAxis(new YAxis
                {
                    Title = new YAxisTitle { Text = "Clients" },
                    Min = 0
                })
                .SetCredits(new Credits
                {
                    Enabled = false
                })
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


            var clientCount = await _database.GetOverallClientCountToday();

            var seriesList = new List<Series>();
            seriesList.Add(new Series() { Name = "Client count", Data = new Data(clientCount.Select(c => new object[] { c.BatchDate, c.Clients }).ToArray()) });

            var chart = new Highcharts("overallclientcount")
                .InitChart(new Chart()
                {
                    DefaultSeriesType = ChartTypes.Spline,
                    ZoomType = ZoomTypes.Xy,
                    Height = 400
                })
                .SetXAxis(new XAxis
                {
                    Type = AxisTypes.Datetime,
                    DateTimeLabelFormats = new DateTimeLabel
                    {
                        Month = "%e %b",
                        Year = "%e %b",
                        Day = "%e %b",
                        Week = "%e %b",
                        Hour = "%I %p",
                        Minute = "%I:%M %p"
                    },
                    LineColor = Color.White,
                    TickColor = Color.White,

                })
                .SetYAxis(new YAxis
                {
                    Title = new YAxisTitle { Text = "Clients" },
                    Min = 0
                })
                .SetCredits(new Credits
                {
                    Enabled = false
                })
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
                .SetSeries(seriesList.ToArray());

            model.OverallClientCount = chart;

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}