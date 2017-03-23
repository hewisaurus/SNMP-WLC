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
            var overall1hTask = _chart.GetOverallClientCount(1);
            var overall2hTask = _chart.GetOverallClientCount(2);
            var overall4hTask = _chart.GetOverallClientCount(4);
            var overall8hTask = _chart.GetOverallClientCount(8);
            var overall16hTask = _chart.GetOverallClientCount(16);
            var overall24hTask = _chart.GetOverallClientCount(24);
            var overall48hTask = _chart.GetOverallClientCount(48);

            //var perVlan1hTask = _chart.GetPerVlanClientCount(1);
            //var perVlan2hTask = _chart.GetPerVlanClientCount(2);
            //var perVlan4hTask = _chart.GetPerVlanClientCount(4);
            //var perVlan8hTask = _chart.GetPerVlanClientCount(8);
            //var perVlan16hTask = _chart.GetPerVlanClientCount(16);
            //var perVlan24hTask = _chart.GetPerVlanClientCount(24);
            //var perVlan48hTask = _chart.GetPerVlanClientCount(48);

            var perAccessPoint1hTask = _chart.GetPerAccessPointClientCount(1);
            var perAccessPoint2hTask = _chart.GetPerAccessPointClientCount(2);
            var perAccessPoint4hTask = _chart.GetPerAccessPointClientCount(4);
            //var perAccessPoint8hTask = _chart.GetPerAccessPointClientCount(8);
            //var perAccessPoint16hTask = _chart.GetPerAccessPointClientCount(16);
            //var perAccessPoint24hTask = _chart.GetPerAccessPointClientCount(24);
            //var perAccessPoint48hTask = _chart.GetPerAccessPointClientCount(48);


            await Task.WhenAll(overall1hTask, overall2hTask, overall4hTask, overall8hTask, overall16hTask, overall24hTask, overall48hTask,
                //perVlan1hTask, perVlan2hTask, perVlan4hTask, perVlan8hTask, perVlan16hTask, perVlan24hTask, perVlan48hTask,
                perAccessPoint1hTask, perAccessPoint2hTask, perAccessPoint4hTask
                
                //,perAccessPoint8hTask, perAccessPoint16hTask, perAccessPoint24hTask, perAccessPoint48hTask
                );



            var model = new IndexViewModel
            {
                
                OverallClientCountLastHour = overall1hTask.Result,
                OverallClientCountLast2Hours = overall2hTask.Result,
                OverallClientCountLast4Hours = overall4hTask.Result,
                OverallClientCountLast8Hours = overall8hTask.Result,
                OverallClientCountLast16Hours = overall16hTask.Result,
                OverallClientCountLast24Hours = overall24hTask.Result,
                OverallClientCountLast48Hours = overall48hTask.Result,

                //PerVlanClientCountLastHour = perVlan1hTask.Result,
                //PerVlanClientCountLast2Hours = perVlan2hTask.Result,
                //PerVlanClientCountLast4Hours = perVlan4hTask.Result,
                //PerVlanClientCountLast8Hours = perVlan8hTask.Result,
                //PerVlanClientCountLast16Hours = perVlan16hTask.Result,
                //PerVlanClientCountLast24Hours = perVlan24hTask.Result,
                //PerVlanClientCountLast48Hours = perVlan48hTask.Result,

                PerAccessPointClientCountLastHour = perAccessPoint1hTask.Result,
                PerAccessPointClientCountLast2Hours = perAccessPoint2hTask.Result,
                PerAccessPointClientCountLast4Hours = perAccessPoint4hTask.Result,
                //PerAccessPointClientCountLast8Hours = perAccessPoint8hTask.Result,
                //PerAccessPointClientCountLast16Hours = perAccessPoint16hTask.Result,
                //PerAccessPointClientCountLast24Hours = perAccessPoint24hTask.Result,
                //PerAccessPointClientCountLast48Hours = perAccessPoint48hTask.Result,
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