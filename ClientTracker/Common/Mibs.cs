using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
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
            ClientVlan,
            ApBaseRadioMacAddress,
            ApName,
            ApLocation,
            ApModel,
            ApSerialNumber,
            ApIpAddress,
            ApEthernetMacAddress
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
                case Mib.ApBaseRadioMacAddress:
                    return "1.3.6.1.4.1.14179.2.2.1.1.1";
                case Mib.ApName:
                    return "1.3.6.1.4.1.14179.2.2.1.1.3";
                case Mib.ApLocation:
                    return "1.3.6.1.4.1.14179.2.2.1.1.4";
                case Mib.ApModel:
                    return "1.3.6.1.4.1.14179.2.2.1.1.16";
                case Mib.ApSerialNumber:
                    return "1.3.6.1.4.1.14179.2.2.1.1.17";
                case Mib.ApIpAddress:
                    return "1.3.6.1.4.1.14179.2.2.1.1.19";
                case Mib.ApEthernetMacAddress:
                    return "1.3.6.1.4.1.14179.2.2.1.1.33";

                case Mib.ClientMacAddress:
                default:
                    return "1.3.6.1.4.1.14179.2.1.4.1.1";
            }
        }
    }
}
