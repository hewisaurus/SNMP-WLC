using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Sql
{
    public class ClientTrackingSql
    {
        public const string Insert =
            "INSERT INTO ClientTracking(ClientId,IpAddressId,Username,AccessPointId,SsidId,WlanInterfaceId,VlanId,BatchDate)" +
            "VALUES(@clientId,@ipAddressId,@username,@accessPointId,@ssidId,@wlanInterfaceId,@vlanId,@batchDate)";

        public const string GetOverallClientCountGreater =
            "SELECT BatchDate,Count(ClientId) AS Clients FROM ClientTracking " +
            "WHERE BatchDate > @batchDate " +
            "GROUP BY BatchDate " +
            "ORDER BY BatchDate ASC";

        public const string GetAccessPointClientCountGreater =
            "SELECT BatchDate, AP.Name AS AccessPoint, Count(ClientId) AS Clients " +
            "FROM ClientTracking CT JOIN AccessPoint AP ON CT.AccessPointId = AP.Id " +
            "JOIN Vlan V ON CT.VlanId = V.Id " +
            "WHERE BatchDate > @batchDate AND V.Value != '600' " +
            "GROUP BY BatchDate, AccessPointId " +
            "ORDER BY BatchDate ASC";

        public const string GetVlanClientCountGreater =
            "SELECT BatchDate, V.Value AS Vlan, Count(ClientId) AS Clients " +
            "FROM ClientTracking CT JOIN Vlan V ON CT.VlanId = V.Id " +
            "WHERE BatchDate > @batchDate " +
            "GROUP BY BatchDate, VlanId " +
            "ORDER BY BatchDate ASC";
    }
}
