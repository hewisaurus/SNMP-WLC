﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common;
using Database;
using SnmpSharpNet;

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
            _writer.WriteLine("Application running!");
        }

        public void UpdateDatabase()
        {
            _writer.WriteLine("Gathering details from SNMP...");

            var macAddresses = GatherBulk(Mibs.GetValue(Mibs.Mib.ClientMacAddress), Connection.Host, Connection.Community);
            var ipAddresses = GatherBulk(Mibs.GetValue(Mibs.Mib.ClientIpAddress), Connection.Host, Connection.Community);
            var usernames = GatherBulk(Mibs.GetValue(Mibs.Mib.ClientUsername), Connection.Host, Connection.Community);
            var apMacAddresses = GatherBulk(Mibs.GetValue(Mibs.Mib.ClientApMacAddress), Connection.Host, Connection.Community);
            var ssids = GatherBulk(Mibs.GetValue(Mibs.Mib.ClientSsid), Connection.Host, Connection.Community);
            var wlanInterfaces = GatherBulk(Mibs.GetValue(Mibs.Mib.ClientWlanInterface), Connection.Host, Connection.Community);
            var vlans = GatherBulk(Mibs.GetValue(Mibs.Mib.ClientVlan), Connection.Host, Connection.Community);

            _writer.WriteLine("Finished retrieving SNMP data");

            var clients = new List<Client>();
            clients.Update(macAddresses, Mibs.Mib.ClientMacAddress);
            clients.Update(ipAddresses, Mibs.Mib.ClientIpAddress);
            clients.Update(usernames, Mibs.Mib.ClientUsername);
            clients.Update(apMacAddresses, Mibs.Mib.ClientApMacAddress);
            clients.Update(ssids, Mibs.Mib.ClientSsid);
            clients.Update(wlanInterfaces, Mibs.Mib.ClientWlanInterface);
            clients.Update(vlans, Mibs.Mib.ClientVlan);

            _writer.WriteLine("Database update started");

            _writer.WriteLine($"We currently have {clients.Count} clients online");

            _writer.WriteLine("SSID Client count:");
            foreach (var ssidGroup in clients.GroupBy(c => c.Ssid))
            {
                _writer.WriteLine($"{ssidGroup.Key}: {ssidGroup.Count()}");
            }

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