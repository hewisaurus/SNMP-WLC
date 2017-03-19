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
            "INSERT INTO ClientTracking(ClientId,IpAddressId,Username,AccessPointId,SsidId,WlanInterfaceId,VlanId)" +
            "VALUES(@clientId,@ipAddressId,@username,@accessPointId,@ssidId,@wlanInterfaceId,@vlanId)";
    }
}
