using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common;
using Database;
using Database.Models;
using SnmpSharpNet;
using IpAddress = SnmpSharpNet.IpAddress;

namespace ClientTracker
{
    public class ClientTracker
    {
        private readonly IConsoleWriter _writer;
        private readonly IDatabaseRepository _database;

        public ClientTracker(IConsoleWriter writer, IDatabaseRepository database)
        {
            _writer = writer;
            _database = database;
        }

        public void Run()
        {
            _writer.WriteLine("Started ClientTracker successfully.");
        }

        /// <summary>
        /// This method polls both the APs and the Clients, and updates the database
        /// </summary>
        public async Task UpdateDatabase()
        {
            _writer.WriteLine("Gathering details from SNMP...");

            var macAddresses = GatherBulk(Mibs.GetValue(Mibs.Mib.ClientMacAddress), Connection.Host, Connection.Community);
            var ipAddresses = GatherBulk(Mibs.GetValue(Mibs.Mib.ClientIpAddress), Connection.Host, Connection.Community);
            var usernames = GatherBulk(Mibs.GetValue(Mibs.Mib.ClientUsername), Connection.Host, Connection.Community);
            var apMacAddresses = GatherBulk(Mibs.GetValue(Mibs.Mib.ClientApMacAddress), Connection.Host, Connection.Community);
            var ssids = GatherBulk(Mibs.GetValue(Mibs.Mib.ClientSsid), Connection.Host, Connection.Community);
            var wlanInterfaces = GatherBulk(Mibs.GetValue(Mibs.Mib.ClientWlanInterface), Connection.Host, Connection.Community);
            var vlans = GatherBulk(Mibs.GetValue(Mibs.Mib.ClientVlan), Connection.Host, Connection.Community);

            var apBaseRadioMac = GatherBulk(Mibs.GetValue(Mibs.Mib.ApBaseRadioMacAddress), Connection.Host, Connection.Community);
            var apEthernetMac = GatherBulk(Mibs.GetValue(Mibs.Mib.ApEthernetMacAddress), Connection.Host, Connection.Community);
            var apIpAddresses = GatherBulk(Mibs.GetValue(Mibs.Mib.ApIpAddress), Connection.Host, Connection.Community);
            var apNames = GatherBulk(Mibs.GetValue(Mibs.Mib.ApName), Connection.Host, Connection.Community);
            var apLocations = GatherBulk(Mibs.GetValue(Mibs.Mib.ApLocation), Connection.Host, Connection.Community);
            var apModels = GatherBulk(Mibs.GetValue(Mibs.Mib.ApModel), Connection.Host, Connection.Community);
            var apSerialNumbers = GatherBulk(Mibs.GetValue(Mibs.Mib.ApSerialNumber), Connection.Host, Connection.Community);

            _writer.WriteLine("Finished retrieving SNMP data");

            var clients = new List<SnmpClient>();
            clients.Update(macAddresses, Mibs.Mib.ClientMacAddress);
            clients.Update(ipAddresses, Mibs.Mib.ClientIpAddress);
            clients.Update(usernames, Mibs.Mib.ClientUsername);
            clients.Update(apMacAddresses, Mibs.Mib.ClientApMacAddress);
            clients.Update(ssids, Mibs.Mib.ClientSsid);
            clients.Update(wlanInterfaces, Mibs.Mib.ClientWlanInterface);
            clients.Update(vlans, Mibs.Mib.ClientVlan);
            _writer.WriteLine($"Pre-cleanup: {clients.Count} clients");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            clients.Cleanup();
            sw.Stop();
            _writer.WriteLine($"Post-cleanup: {clients.Count} clients");
            _writer.WriteLine($"SNMP clients updated and cleaned up in {sw.Elapsed}");


            var accessPoints = new List<SnmpAccessPoint>();
            accessPoints.Update(apBaseRadioMac, Mibs.Mib.ApBaseRadioMacAddress);
            accessPoints.Update(apEthernetMac, Mibs.Mib.ApEthernetMacAddress);
            accessPoints.Update(apIpAddresses, Mibs.Mib.ApIpAddress);
            accessPoints.Update(apNames, Mibs.Mib.ApName);
            accessPoints.Update(apLocations, Mibs.Mib.ApLocation);
            accessPoints.Update(apModels, Mibs.Mib.ApModel);
            accessPoints.Update(apSerialNumbers, Mibs.Mib.ApSerialNumber);

            _writer.WriteLine("Database update started");
            
            // Update AP Models
            var updateResult = await _database.UpdateAccessPointModels(accessPoints.Select(ap => ap.Model).Distinct().ToList());
            _writer.WriteLine(!updateResult.Success
                ? $"Something went wrong updating access point models.. {updateResult.Message}"
                : $"Updated AP Models in {updateResult.TimeTaken}");

            // Update SSIDs
            updateResult = await _database.UpdateSsids(clients.Select(c => c.Ssid).Distinct().ToList());
            _writer.WriteLine(!updateResult.Success
                ? $"Something went wrong updating SSIDs.. {updateResult.Message}"
                : $"Updated SSIDs in {updateResult.TimeTaken}");

            // Update VLANs
            updateResult = await _database.UpdateVlans(clients.Where(c => !string.IsNullOrEmpty(c.Vlan)).Select(c => c.Vlan).Distinct().ToList());
            _writer.WriteLine(!updateResult.Success
                ? $"Something went wrong updating VLANs.. {updateResult.Message}"
                : $"Updated VLANs in {updateResult.TimeTaken}");

            // Update interfaces
            updateResult = await _database.UpdateWlanInterfaces(clients.Where(c => !string.IsNullOrEmpty(c.Interface)).Select(c => c.Interface).Distinct().ToList());
            _writer.WriteLine(!updateResult.Success
                ? $"Something went wrong updating WLC interfaces.. {updateResult.Message}"
                : $"Updated WLC interfaces in {updateResult.TimeTaken}");

            // Update clients
            updateResult = await _database.UpdateClients(clients.Where(c => !string.IsNullOrEmpty(c.MacAddress)).Select(c => c.MacAddress.Replace(" ","")).Distinct().ToList());
            _writer.WriteLine(!updateResult.Success
                ? $"Something went wrong updating clients.. {updateResult.Message}"
                : $"Updated clients in {updateResult.TimeTaken}");

            // Update IP Addresses
            updateResult = await _database.UpdateIpAddresses(clients.Where(c => !string.IsNullOrEmpty(c.IpAddress)).Select(c => c.IpAddress).Distinct().ToList());
            _writer.WriteLine(!updateResult.Success
                ? $"Something went wrong updating IP addresses.. {updateResult.Message}"
                : $"Updated IP addresses in {updateResult.TimeTaken}");

            var dbApModels = await _database.GetApModelsAsync();
            // Build the list of access points that the database should contain
            var actualAps = accessPoints.Select(accessPoint => new AccessPoint
            {
                BaseRadioMacAddress = accessPoint.BaseRadioMacAddress,
                EthernetMacAddress = accessPoint.EthernetMacAddress,
                Location = accessPoint.Location,
                Name = accessPoint.Name,
                ModelId = dbApModels.First(m => m.Name == accessPoint.Model).Id,
                IpAddress = accessPoint.IpAddress
            }).ToList();
            updateResult = await _database.UpdateAccessPoints(actualAps);
            _writer.WriteLine(!updateResult.Success
                ? $"Something went wrong updating access points.. {updateResult.Message}"
                : $"Updated APs in {updateResult.TimeTaken}");

            // Return the records so that we can key them properly (i.e. can't insert without FKs)
            var apListTask = _database.GetAccessPointsAsync();
            var clientListTask = _database.GetClientsAsync();
            var ssidListTask = _database.GetSsidsAsync();
            var vlanListTask = _database.GetVlansAsync();
            var wlanInterfaceListTask = _database.GetWlanInterfacesAsync();
            var ipAddressListTask = _database.GetIpAddressesAsync();

            await Task.WhenAll(apListTask, clientListTask, ssidListTask, vlanListTask, wlanInterfaceListTask, ipAddressListTask);
            var apList = apListTask.Result;
            var clientList = clientListTask.Result;
            var ssidList = ssidListTask.Result;
            var vlanList = vlanListTask.Result;
            var wlanInterfaceList = wlanInterfaceListTask.Result;
            var ipAddressList = ipAddressListTask.Result;

            // Add client tracking records
            var clientTrackingList = new List<ClientTracking>();
            var batchDate = DateTime.Now;
            foreach (var client in clients)
            {
                try
                {
                    clientTrackingList.Add(new ClientTracking
                    {
                        ClientId = clientList.First(c => c.MacAddress == client.MacAddress.Replace(" ", "").ToUpper()).Id,
                        AccessPointId = apList.First(ap => ap.BaseRadioMacAddress == client.ApMacAddress.Replace(" ", "").ToUpper()).Id,
                        IpAddressId = ipAddressList.First(ip => ip.Value == client.IpAddress).Id,
                        SsidId = ssidList.First(s => s.Value == client.Ssid).Id,
                        Username = client.Username,
                        VlanId = vlanList.First(v => v.Value == client.Vlan).Id,
                        WlanInterfaceId = wlanInterfaceList.First(w => w.Value == client.Interface).Id,
                        BatchDate = batchDate
                    });
                }
                catch (NullReferenceException nre)
                {
                    _writer.WriteLine("Hit a null reference exception for the following client...");
                    _writer.WriteLine(client.ToString());
                }
                catch (Exception ex)
                {
                   _writer.WriteLine($"Threw an exception for the following client ({ex.Message})");
                    _writer.WriteLine(client.ToString());
                }
                
            }
            
            _writer.WriteLine("Beginning main update block");

            updateResult = await _database.AddClientTracking(clientTrackingList);
            _writer.WriteLine(!updateResult.Success
               ? $"Something went wrong adding client tracking.. {updateResult.Message}"
               : $"Main update block completed successfully in {updateResult.TimeTaken}");

            _writer.WriteLine("Database update complete");
        }

        static List<BulkResult> GatherBulk(string oid, string host, string communityString)
        {
            var returnValue = new List<BulkResult>();

            // SNMP community name
            OctetString community = new OctetString(communityString);

            // Define agent parameters class
            AgentParameters param = new AgentParameters(community);
            // Set SNMP version to 2 (GET-BULK only works with SNMP ver 2 and 3)
            param.Version = SnmpVersion.Ver2;
            // Construct the agent address object
            // IpAddress class is easy to use here because
            //  it will try to resolve constructor parameter if it doesn't
            //  parse to an IP address
            IpAddress agent = new IpAddress(host);

            // Construct target
            UdpTarget target = new UdpTarget((IPAddress)agent, 161, 2000, 1);

            // Define Oid that is the root of the MIB
            //  tree you wish to retrieve
            Oid rootOid = new Oid(oid);

            // This Oid represents last Oid returned by
            //  the SNMP agent
            Oid lastOid = (Oid)rootOid.Clone();

            // Pdu class used for all requests
            Pdu pdu = new Pdu(PduType.GetBulk);

            // In this example, set NonRepeaters value to 0
            pdu.NonRepeaters = 0;
            // MaxRepetitions tells the agent how many Oid/Value pairs to return
            // in the response.
            pdu.MaxRepetitions = 5;

            // Loop through results
            while (lastOid != null)
            {
                // When Pdu class is first constructed, RequestId is set to 0
                // and during encoding id will be set to the random value
                // for subsequent requests, id will be set to a value that
                // needs to be incremented to have unique request ids for each
                // packet
                if (pdu.RequestId != 0)
                {
                    pdu.RequestId += 1;
                }
                // Clear Oids from the Pdu class.
                pdu.VbList.Clear();
                // Initialize request PDU with the last retrieved Oid
                pdu.VbList.Add(lastOid);
                // Make SNMP request
                SnmpV2Packet result = (SnmpV2Packet)target.Request(pdu, param);
                // You should catch exceptions in the Request if using in real application.

                // If result is null then agent didn't reply or we couldn't parse the reply.
                if (result != null)
                {
                    // ErrorStatus other then 0 is an error returned by 
                    // the Agent - see SnmpConstants for error definitions
                    if (result.Pdu.ErrorStatus != 0)
                    {
                        // agent reported an error with the request
                        Console.WriteLine("Error in SNMP reply. Error {0} index {1}",
                            result.Pdu.ErrorStatus,
                            result.Pdu.ErrorIndex);
                        lastOid = null;
                        break;
                    }
                    else
                    {
                        // Walk through returned variable bindings
                        foreach (Vb v in result.Pdu.VbList)
                        {
                            // Check that retrieved Oid is "child" of the root OID
                            if (rootOid.IsRootOf(v.Oid))
                            {
                                //returnValue.Add(v.Value.ToString());
                                returnValue.Add(new BulkResult(v.Oid.ToString(), v.Value.ToString()));
                                //Console.WriteLine("{0} ({1}): {2}",
                                //    v.Oid.ToString(),
                                //    SnmpConstants.GetTypeName(v.Value.Type),
                                //    v.Value.ToString());
                                if (v.Value.Type == SnmpConstants.SMI_ENDOFMIBVIEW)
                                    lastOid = null;
                                else
                                    lastOid = v.Oid;
                            }
                            else
                            {
                                // we have reached the end of the requested
                                // MIB tree. Set lastOid to null and exit loop
                                lastOid = null;
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No response received from SNMP agent.");
                }
            }
            target.Close();
            return returnValue;
        }
    }
}
