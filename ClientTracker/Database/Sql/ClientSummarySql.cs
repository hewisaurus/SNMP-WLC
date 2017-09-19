using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Sql
{
    public class ClientSummarySql
    {
        public const string GetRecordsForDateRange =
            "SELECT * FROM ClientSummary WHERE Date > @startDate AND Date < @endDate";

        // Queries before date was a FK
        //public const string GetClientSummaryForDateRange =
        //    "SELECT Date, SUM(ClientCount) AS ClientCount FROM ClientSummary WHERE Date > @startDate AND Date < @endDate GROUP BY Date";
        //public const string GetClientSummaryForDateRangeAndAccessPoint =
        //    "SELECT Date, SUM(ClientCount) AS ClientCount FROM ClientSummary WHERE Date > @startDate AND Date < @endDate AND AccessPointId = @accessPointId GROUP BY Date";
        //public const string GetClientSummaryForDateRangeAndSsid =
        //    "SELECT Date, SUM(ClientCount) AS ClientCount FROM ClientSummary WHERE Date > @startDate AND Date < @endDate AND SsidId = @ssidId GROUP BY Date";
        //public const string GetClientRecordsForDateRange =
        //    "SELECT Date, ClientCount FROM ClientSummary WHERE Date > @startDate AND Date < @endDate";

        public const string GetClientSummaryForDateRange =
            "SELECT BD.Date, CSO.ClientCount FROM ClientSummaryOverall CSO " +
            "JOIN BatchDate BD ON CSO.BatchDateId = BD.Id " +
            "WHERE BD.Date > @startDate AND BD.Date < @endDate ORDER BY BD.Date;";

        public const string GetClientSummaryForDateRangeAndAccessPoint =
            "SELECT BD.Date, CSAP.ClientCount FROM ClientSummaryAccessPoint CSAP " +
            "JOIN BatchDate BD ON CSAP.BatchDateId = BD.Id " +
            "WHERE BD.Date > @startDate AND BD.Date < @endDate AND CSAP.AccessPointId = @accessPointId ORDER BY BD.Date";

        public const string GetClientSummaryForDateRangeAndVlan =
            "SELECT BD.Date, CSVL.ClientCount FROM ClientSummaryVlan CSVL " +
            "JOIN BatchDate BD ON CSVL.BatchDateId = BD.Id " +
            "WHERE BD.Date > @startDate AND BD.Date < @endDate AND CSVL.VlanId = @vlanId ORDER BY BD.Date";
    }
}
