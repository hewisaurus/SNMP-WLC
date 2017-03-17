using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    public static class Extensions
    {
        public static void Update(this List<Client> clients, List<BulkResult> results, Mibs.Mib mib)
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
                            clients.Add(new Client(id) { ApMacAddress = result.Value });
                            break;
                        case Mibs.Mib.ClientIpAddress:
                            clients.Add(new Client(id) { IpAddress = result.Value });
                            break;
                        case Mibs.Mib.ClientMacAddress:
                            clients.Add(new Client(id) { MacAddress = result.Value });
                            break;
                        case Mibs.Mib.ClientSsid:
                            clients.Add(new Client(id) { Ssid = result.Value });
                            break;
                        case Mibs.Mib.ClientUsername:
                            clients.Add(new Client(id) { Username = result.Value });
                            break;
                        case Mibs.Mib.ClientVlan:
                            clients.Add(new Client(id) { Vlan = result.Value });
                            break;
                        case Mibs.Mib.ClientWlanInterface:
                            clients.Add(new Client(id) { Interface = result.Value });
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
    }
}
