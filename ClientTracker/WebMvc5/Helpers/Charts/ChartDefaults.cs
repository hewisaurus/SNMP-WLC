using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNet.Highcharts.Enums;

namespace WebMvc5.Helpers.Charts
{
    public class ChartDefaults
    {
        public const ChartTypes OverallClientCountChartType = ChartTypes.Spline;
        public const ZoomTypes OverallClientCountZoomType = ZoomTypes.Xy;
        public const int OverallClientCountChartHeight = 400;

        public const string OverallClientCountDateTimeLabelFormatMonth = "%e %b";
        public const string OverallClientCountDateTimeLabelFormatYear = "%e %b";
        public const string OverallClientCountDateTimeLabelFormatDay = "%e %b";
        public const string OverallClientCountDateTimeLabelFormatWeek = "%e %b";
        public const string OverallClientCountDateTimeLabelFormatHour = "%I %p";
        public const string OverallClientCountDateTimeLabelFormatMinute = "%I:%M %p";
    }
}