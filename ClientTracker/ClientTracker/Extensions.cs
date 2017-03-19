using System.Collections.Generic;
using System.Linq;
using Common;
using Database.Models;

namespace ClientTracker
{
    public static class Extensions
    {
        public static void Update(this List<SnmpClient> clients, List<BulkResult> results, Mibs.Mib mib)
        {
            foreach (var result in results)
            {
                var id = result.Mib.Replace($"{Mibs.GetValue(mib)}.", "");
                var client = clients.FirstOrDefault(c => c.Index == id);
                if (client == null)
                {
                    // Client doesn't exist in this list yet
                    switch (mib)
                    {
                        case Mibs.Mib.ClientApMacAddress:
                            clients.Add(new SnmpClient(id) { ApMacAddress = result.Value });
                            break;
                        case Mibs.Mib.ClientIpAddress:
                            clients.Add(new SnmpClient(id) { IpAddress = result.Value });
                            break;
                        case Mibs.Mib.ClientMacAddress:
                            clients.Add(new SnmpClient(id) { MacAddress = result.Value });
                            break;
                        case Mibs.Mib.ClientSsid:
                            clients.Add(new SnmpClient(id) { Ssid = result.Value });
                            break;
                        case Mibs.Mib.ClientUsername:
                            clients.Add(new SnmpClient(id) { Username = result.Value });
                            break;
                        case Mibs.Mib.ClientVlan:
                            clients.Add(new SnmpClient(id) { Vlan = result.Value });
                            break;
                        case Mibs.Mib.ClientWlanInterface:
                            clients.Add(new SnmpClient(id) { Interface = result.Value });
                            break;
                    }
                }
                else
                {
                    switch (mib)
                    {
                        case Mibs.Mib.ClientApMacAddress:
                            client.ApMacAddress = result.Value;
                            break;
                        case Mibs.Mib.ClientIpAddress:
                            client.IpAddress = result.Value;
                            break;
                        case Mibs.Mib.ClientMacAddress:
                            client.MacAddress = result.Value;
                            break;
                        case Mibs.Mib.ClientSsid:
                            client.Ssid = result.Value;
                            break;
                        case Mibs.Mib.ClientUsername:
                            client.Username = result.Value;
                            break;
                        case Mibs.Mib.ClientVlan:
                            client.Vlan = result.Value;
                            break;
                        case Mibs.Mib.ClientWlanInterface:
                            client.Interface = result.Value;
                            break;
                    }
                }
            }
        }

        public static void Update(this List<SnmpAccessPoint> accessPoints, List<BulkResult> results, Mibs.Mib mib)
        {
            foreach (var result in results)
            {
                var id = result.Mib.Replace($"{Mibs.GetValue(mib)}.", "");
                var accessPoint = accessPoints.FirstOrDefault(c => c.Index == id);
                if (accessPoint == null)
                {
                    // AccessPoint doesn't exist in this list yet
                    switch (mib)
                    {
                        case Mibs.Mib.ApName:
                            accessPoints.Add(new SnmpAccessPoint(id) { Name = result.Value });
                            break;
                        case Mibs.Mib.ApBaseRadioMacAddress:
                            accessPoints.Add(new SnmpAccessPoint(id) { BaseRadioMacAddress = result.Value });
                            break;
                        case Mibs.Mib.ApEthernetMacAddress:
                            accessPoints.Add(new SnmpAccessPoint(id) { EthernetMacAddress = result.Value });
                            break;
                        case Mibs.Mib.ApIpAddress:
                            accessPoints.Add(new SnmpAccessPoint(id) { IpAddress = result.Value });
                            break;
                        case Mibs.Mib.ApLocation:
                            accessPoints.Add(new SnmpAccessPoint(id) { Location = result.Value });
                            break;
                        case Mibs.Mib.ApModel:
                            accessPoints.Add(new SnmpAccessPoint(id) { Model = result.Value });
                            break;
                        case Mibs.Mib.ApSerialNumber:
                            accessPoints.Add(new SnmpAccessPoint(id) { SerialNumber = result.Value });
                            break;
                    }
                }
                else
                {
                    switch (mib)
                    {
                        case Mibs.Mib.ApName:
                            accessPoint.Name = result.Value;
                            break;
                        case Mibs.Mib.ApBaseRadioMacAddress:
                            accessPoint.BaseRadioMacAddress = result.Value;
                            break;
                        case Mibs.Mib.ApEthernetMacAddress:
                            accessPoint.EthernetMacAddress = result.Value;
                            break;
                        case Mibs.Mib.ApIpAddress:
                            accessPoint.IpAddress = result.Value;
                            break;
                        case Mibs.Mib.ApLocation:
                            accessPoint.Location = result.Value;
                            break;
                        case Mibs.Mib.ApModel:
                            accessPoint.Model = result.Value;
                            break;
                        case Mibs.Mib.ApSerialNumber:
                            accessPoint.SerialNumber = result.Value;
                            break;
                    }
                }
            }
        }
    }
}
