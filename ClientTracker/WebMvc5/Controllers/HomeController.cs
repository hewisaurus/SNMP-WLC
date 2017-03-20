using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;

namespace WebMvc5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var rec = new List<Tuple<DateTime, int>>
            {
                new Tuple<DateTime, int>(new DateTime(2017, 1, 1), 35),
                new Tuple<DateTime, int>(new DateTime(2017, 1, 2), 37),
                new Tuple<DateTime, int>(new DateTime(2017, 1, 3), 12),
                new Tuple<DateTime, int>(new DateTime(2017, 1, 4), 53),
                new Tuple<DateTime, int>(new DateTime(2017, 1, 5), 26)
            };

            var seriesList = new List<Series>();
            seriesList.Add(new Series() { Name = "s1", Data = new Data(rec.Select(r => new object[] { r.Item1, r.Item2 }).ToArray()) });

            var chart = new Highcharts("test1")
                .InitChart(new Chart()
                {
                    DefaultSeriesType = ChartTypes.Spline,
                    ZoomType = ZoomTypes.Xy,
                    Height = 400
                })
                .SetXAxis(new XAxis
                {
                    Type = AxisTypes.Datetime,
                    DateTimeLabelFormats = new DateTimeLabel { Month = "%e %b", Year = "%e %b", Day = "%e %b", Week = "%e %b" },
                    LineColor = Color.White,
                    TickColor = Color.White,

                })
                .SetSeries(seriesList.ToArray());

            return View(chart);
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