using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNet.Highcharts;

namespace WebMvc5.Models.Home
{
    public class IndexViewModel
    {
        public Highcharts OverallClientCount { get; set; }
        public Highcharts AccessPointClientCount { get; set; }

        public Highcharts OverallClientCountLastHour { get; set; }
        public Highcharts OverallClientCountLast2Hours { get; set; }
        public Highcharts OverallClientCountLast4Hours { get; set; }
        public Highcharts OverallClientCountLast8Hours { get; set; }
        public Highcharts OverallClientCountLast16Hours { get; set; }
        public Highcharts OverallClientCountLast24Hours { get; set; }
        public Highcharts OverallClientCountLast48Hours { get; set; }
    }
}