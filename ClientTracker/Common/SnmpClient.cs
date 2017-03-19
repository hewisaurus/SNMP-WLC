using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SnmpClient
    {
        public string Index { get; set; }
        public string MacAddress { get; set; }
        public string IpAddress { get; set; }
        public string Username { get; set; }
        public string ApMacAddress { get; set; }
        public string Ssid { get; set; }
        public string Interface { get; set; }
        public string Vlan { get; set; }

        public SnmpClient()
        {

        }

        public SnmpClient(string index)
        {
            Index = index;
        }

        public SnmpClient(string index, string mac, string ip, string user, string apMac, string ssid, string iface, string vlan)
        {
            Index = index;
            MacAddress = mac.Replace(" ","");
            IpAddress = ip;
            Username = user;
            ApMacAddress = apMac.Replace(" ", "");
            Ssid = ssid;
            Interface = iface;
            Vlan = vlan;
        }

        public override string ToString()
        {
            return string.Format(
                "Mac address: {1}{0}" +
                "IP Address: {2}{0}" +
                "Username: {3}{0}" +
                "AP Mac Address: {4}{0}" +
                "SSID: {5}{0}" +
                "Interface: {6}{0}" +
                "VLAN: {7}{0}",
                Environment.NewLine, MacAddress, IpAddress, Username, ApMacAddress, Ssid, Interface, Vlan);
        }
    }
}
