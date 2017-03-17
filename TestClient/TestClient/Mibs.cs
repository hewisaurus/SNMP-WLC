using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    //public class Mibs
    //{
    //    public const string ClientMacAddress = "1.3.6.1.4.1.14179.2.1.4.1.1";
    //    public const string ClientIpAddress = "1.3.6.1.4.1.14179.2.1.4.1.2";
    //    public const string ClientUsername = "1.3.6.1.4.1.14179.2.1.4.1.3";
    //    public const string ClientApMacAddress = "1.3.6.1.4.1.14179.2.1.4.1.4";
    //    public const string ClientSsid = "1.3.6.1.4.1.14179.2.1.4.1.7";
    //    public const string ClientWlanInterface = "1.3.6.1.4.1.14179.2.1.4.1.27";
    //    public const string ClientVlan = "1.3.6.1.4.1.14179.2.1.4.1.29";
    //}

    public class Mibs
    {
        public enum Mib
        {
            ClientMacAddress,
            ClientIpAddress,
            ClientUsername,
            ClientApMacAddress,
            ClientSsid,
            ClientWlanInterface,
            ClientVlan
        }

        public static string GetValue(Mib mib)
        {
            switch (mib)
            {
                    case Mib.ClientApMacAddress:
                    return "1.3.6.1.4.1.14179.2.1.4.1.4";
                case Mib.ClientIpAddress:
                    return "1.3.6.1.4.1.14179.2.1.4.1.2";
                case Mib.ClientSsid:
                    return "1.3.6.1.4.1.14179.2.1.4.1.7";
                case Mib.ClientUsername:
                    return "1.3.6.1.4.1.14179.2.1.4.1.3";
                case Mib.ClientVlan:
                    return "1.3.6.1.4.1.14179.2.1.4.1.29";
                case Mib.ClientWlanInterface:
                    return "1.3.6.1.4.1.14179.2.1.4.1.27";
                case Mib.ClientMacAddress:
                default:
                    return "1.3.6.1.4.1.14179.2.1.4.1.1";
            }
        }
    }

    


}
