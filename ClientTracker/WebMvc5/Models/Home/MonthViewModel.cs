using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNet.Highcharts;

namespace WebMvc5.Models.Home
{
    public class MonthViewModel
    {
        public List<Highcharts> Charts { get; set; }

        public MonthViewModel()
        {
            Charts = new List<Highcharts>();
        }
    }
}