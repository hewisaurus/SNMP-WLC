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

        public Highcharts PerVlanClientCountLastHour { get; set; }
        public Highcharts PerVlanClientCountLast2Hours { get; set; }
        public Highcharts PerVlanClientCountLast4Hours { get; set; }
        public Highcharts PerVlanClientCountLast8Hours { get; set; }
        public Highcharts PerVlanClientCountLast16Hours { get; set; }
        public Highcharts PerVlanClientCountLast24Hours { get; set; }
        public Highcharts PerVlanClientCountLast48Hours { get; set; }

        public Highcharts PerAccessPointClientCountLastHour { get; set; }
        public Highcharts PerAccessPointClientCountLast2Hours { get; set; }
        public Highcharts PerAccessPointClientCountLast4Hours { get; set; }
        public Highcharts PerAccessPointClientCountLast8Hours { get; set; }
        public Highcharts PerAccessPointClientCountLast16Hours { get; set; }
        public Highcharts PerAccessPointClientCountLast24Hours { get; set; }
        public Highcharts PerAccessPointClientCountLast48Hours { get; set; }
    }
}