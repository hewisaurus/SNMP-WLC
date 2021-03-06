﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //    var chart = new Highcharts()
            //    {
            //        ID = "test",
            //        SplineSeries = new SplineSeries()
            //        {
            //            Name = "testSpline",Data = new List<SplineSeriesData>()
            //            {
            //                new SplineSeriesData(){Name = "a",X = 10,Y=20}
            //            }
            //        }
            //    };

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
                .SetSeries(seriesList.ToArray());

            return View(chart);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
