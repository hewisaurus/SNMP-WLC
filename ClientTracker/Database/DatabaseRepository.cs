using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Dapper;
using Database.Connection;
using Database.Models;
using Database.Models.Custom;
using Database.Sql;

namespace Database
{
    public class DatabaseRepository : RepositoryBase, IDatabaseRepository
    {
        public DatabaseRepository(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
        }

        public void LogLine(string line)
        {
            Console.WriteLine(line);
        }

        public async Task<ReturnValue> UpdateAccessPointModels(List<string> models)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                using (var conn = await OpenConnectionAsync())
                {
                    // Search for all models at the same time and compare lists later, rather than one query at a time
                    var existingModels =
                        (await conn.QueryAsync<AccessPointModel>(AccessPointModelSql.GetAllByName, new { names = models }))
                        .ToList();

                    // Compare the list from the DB to what we have here
                    foreach (var model in models)
                    {
                        if (existingModels.All(m => m.Name != model))
                        {
                            // Didn't find it in the DB
                            await conn.ExecuteAsync(AccessPointModelSql.Insert, new { name = model });
                        }
                        //var dbModel =
                        //    (await conn.QueryAsync<AccessPointModel>(AccessPointModelSql.GetByName, new {Name = model}))
                        //    .SingleOrDefault();
                        //if (dbModel == null)
                        //{
                        //    await conn.ExecuteAsync(AccessPointModelSql.Insert, new {name = model});
                        //}
                    }
                }

                sw.Stop();

                return new ReturnValue(true, null, null, sw.Elapsed);
            }
            catch (Exception ex)
            {
                return new ReturnValue(false, ex.Message);
            }
        }


        public async Task<ReturnValue> UpdateAccessPoints(List<AccessPoint> accessPoints)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                using (var conn = await OpenConnectionAsync())
                {
                    // Get the list of IDs that exist within the DB
                    var existingAps =
                       (await conn.QueryAsync<AccessPoint>(AccessPointSql.GetAllByEthernetMac,
                       new { ethernetMacAddresses = accessPoints.Select(ap => ap.EthernetMacAddress.Replace(" ", "").ToUpper()).ToList() }))
                       .ToList();

                    foreach (var accessPoint in accessPoints)
                    {
                        //var dbModel =
                        //    (await conn.QueryAsync<int>(AccessPointSql.GetIdByEthernetMac,
                        //        new { ethernetMacAddress = accessPoint.EthernetMacAddress.Replace(" ", "").ToUpper() }))
                        //    .SingleOrDefault();
                        var dbAp = existingAps.FirstOrDefault(ap => ap.EthernetMacAddress == accessPoint.EthernetMacAddress.Replace(" ", "").ToUpper());
                        if (dbAp == null)
                        {
                            // This AP doesn't exist in the database                        
                            await conn.ExecuteAsync(AccessPointSql.Insert, new
                            {
                                accessPoint.Name,
                                ethernetMacAddress = accessPoint.EthernetMacAddress.Replace(" ", "").ToUpper(),
                                baseRadioMacAddress = accessPoint.BaseRadioMacAddress.Replace(" ", "").ToUpper(),
                                accessPoint.IpAddress,
                                accessPoint.Location,
                                accessPoint.ModelId
                            });
                        }
                        else
                        {
                            await conn.ExecuteAsync(AccessPointSql.Update, new
                            {
                                accessPoint.Name,
                                accessPoint.IpAddress,
                                accessPoint.Location,
                                id = dbAp.Id,
                                lastSeen = DateTime.Now
                            });
                        }
                    }
                }

                sw.Stop();

                return new ReturnValue(true, null, null, sw.Elapsed);
            }
            catch (Exception ex)
            {
                return new ReturnValue(false, ex.Message);
            }
        }

        public async Task<ReturnValue> UpdateSsids(List<string> ssids)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                using (var conn = await OpenConnectionAsync())
                {
                    var existingSsids =
                        (await conn.QueryAsync<Ssid>(SsidSql.GetAllByName, new { values = ssids }))
                        .ToList();

                    foreach (var ssid in ssids)
                    {
                        //var dbModel =
                        //    (await conn.QueryAsync<Ssid>(SsidSql.GetByName, new { value = ssid }))
                        //    .SingleOrDefault();
                        var dbModel = existingSsids.FirstOrDefault(s => s.Value == ssid);
                        if (dbModel == null)
                        {
                            await conn.ExecuteAsync(SsidSql.Insert, new { value = ssid });
                        }
                        else
                        {
                            await conn.ExecuteAsync(SsidSql.Update,
                                new { value = ssid, lastSeen = DateTime.Now, dbModel.Id });
                        }
                    }
                }

                sw.Stop();

                return new ReturnValue(true, null, null, sw.Elapsed);
            }
            catch (Exception ex)
            {
                return new ReturnValue(false, ex.Message);
            }
        }

        public async Task<ReturnValue> UpdateVlans(List<string> vlans)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                using (var conn = await OpenConnectionAsync())
                {
                    var existingVlans =
                        (await conn.QueryAsync<Vlan>(VlanSql.GetAllByName, new { values = vlans }))
                        .ToList();

                    foreach (var vlan in vlans)
                    {
                        //var dbModel =
                        //    (await conn.QueryAsync<Vlan>(VlanSql.GetByName, new { value = vlan }))
                        //    .SingleOrDefault();
                        var dbModel = existingVlans.FirstOrDefault(v => v.Value == vlan);
                        if (dbModel == null)
                        {
                            await conn.ExecuteAsync(VlanSql.Insert, new { value = vlan });
                        }
                        else
                        {
                            await conn.ExecuteAsync(VlanSql.Update,
                                new { value = vlan, lastSeen = DateTime.Now, dbModel.Id });
                        }
                    }
                }

                sw.Stop();

                return new ReturnValue(true, null, null, sw.Elapsed);
            }
            catch (Exception ex)
            {
                return new ReturnValue(false, ex.Message);
            }
        }

        public async Task<ReturnValue> UpdateWlanInterfaces(List<string> interfaces)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                using (var conn = await OpenConnectionAsync())
                {
                    var existingInterfaces =
                        (await conn.QueryAsync<WlanInterface>(WlanInterfaceSql.GetAllByName, new { values = interfaces }))
                        .ToList();

                    foreach (var iface in interfaces)
                    {
                        //var dbModel =
                        //    (await conn.QueryAsync<WlanInterface>(WlanInterfaceSql.GetByName, new { value = iface }))
                        //    .SingleOrDefault();
                        var dbModel = existingInterfaces.FirstOrDefault(v => v.Value == iface);
                        if (dbModel == null)
                        {
                            await conn.ExecuteAsync(WlanInterfaceSql.Insert, new { value = iface });
                        }
                        else
                        {
                            await conn.ExecuteAsync(WlanInterfaceSql.Update,
                                new { value = iface, lastSeen = DateTime.Now, dbModel.Id });
                        }
                    }
                }

                sw.Stop();

                return new ReturnValue(true, null, null, sw.Elapsed);
            }
            catch (Exception ex)
            {
                return new ReturnValue(false, ex.Message);
            }
        }

        public async Task<ReturnValue> UpdateClients(List<string> clients)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var dbClients = new List<Client>();
            List<string> searchClients = new List<string>(clients);
            int maxPerIteration = 950;
            try
            {
                using (var conn = await OpenConnectionAsync())
                {
                    while (searchClients.Any())
                    {
                        List<string> clientsProcessed = searchClients.Count < maxPerIteration
                                ? searchClients.Take(searchClients.Count).Select(c => c.Replace(" ","").ToUpper()).ToList()
                                : searchClients.Take(maxPerIteration).Select(c => c.Replace(" ", "").ToUpper()).ToList();
                        
                        dbClients.AddRange(
                        (await conn.QueryAsync<Client>(ClientSql.GetAllByAddress,
                            new {macAddresses = clientsProcessed})).ToList());

                        searchClients.RemoveRange(0, clientsProcessed.Count);
                    }

                    foreach (var client in clients)
                    {
                        var client1 = client.Replace(" ", "").ToUpper();
                        var dbModel = dbClients.FirstOrDefault(c => c.MacAddress == client1);
                        if (dbModel == null)
                        {
                            await conn.ExecuteAsync(ClientSql.Insert, new { macAddress = client1 });
                        }
                        else
                        {
                            await conn.ExecuteAsync(ClientSql.Update,
                                new { lastSeen = DateTime.Now, dbModel.Id });
                        }
                    }
                }

                sw.Stop();

                return new ReturnValue(true, null, null, sw.Elapsed);
            }
            catch (Exception ex)
            {
                return new ReturnValue(false, ex.Message);
            }
        }

        public async Task<ReturnValue> UpdateIpAddresses(List<string> addresses)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            var dbAddresses = new List<IpAddress>();
            List<string> searchAddresses = new List<string>(addresses);
            int maxPerIteration = 950;
            try
            {
                using (var conn = await OpenConnectionAsync())
                {
                    while (searchAddresses.Any())
                    {
                        List<string> addressesProcessed = searchAddresses.Count < maxPerIteration
                                ? searchAddresses.Take(searchAddresses.Count).Select(c => c.Replace(" ", "").ToUpper()).ToList()
                                : searchAddresses.Take(maxPerIteration).Select(c => c.Replace(" ", "").ToUpper()).ToList();

                        dbAddresses.AddRange(
                        (await conn.QueryAsync<IpAddress>(IpAddressSql.GetAllByAddress,
                            new { values = addressesProcessed })).ToList());

                        searchAddresses.RemoveRange(0, addressesProcessed.Count);
                    }

                    foreach (var address in addresses)
                    {
                        var dbModel = dbAddresses.FirstOrDefault(c => c.Value == address);
                        if (dbModel == null)
                        {
                            await conn.ExecuteAsync(IpAddressSql.Insert, new { value = address });
                        }
                        else
                        {
                            await conn.ExecuteAsync(IpAddressSql.Update,
                                new { lastSeen = DateTime.Now, dbModel.Id });
                        }
                    }
                }

                sw.Stop();

                return new ReturnValue(true, null, null, sw.Elapsed);
            }
            catch (Exception ex)
            {
                return new ReturnValue(false, ex.Message);
            }
        }

        public async Task<List<AccessPointModel>> GetApModelsAsync()
        {
            return (await QueryAsync(q => q.QueryAsync<AccessPointModel>(AccessPointModelSql.GetAll))).ToList();
        }

        public async Task<List<AccessPoint>> GetAccessPointsAsync()
        {
            return (await QueryAsync(q => q.QueryAsync<AccessPoint, AccessPointModel, AccessPoint>
            (AccessPointSql.GetAll, (ap, apm) =>
            {
                ap.Model = apm;
                return ap;
            }))).ToList();
        }

        public async Task<List<Ssid>> GetSsidsAsync()
        {
            return (await QueryAsync(q => q.QueryAsync<Ssid>(SsidSql.GetAll))).ToList();
        }

        public async Task<List<Vlan>> GetVlansAsync()
        {
            return (await QueryAsync(q => q.QueryAsync<Vlan>(VlanSql.GetAll))).ToList();
        }

        public async Task<List<WlanInterface>> GetWlanInterfacesAsync()
        {
            return (await QueryAsync(q => q.QueryAsync<WlanInterface>(WlanInterfaceSql.GetAll))).ToList();
        }

        public async Task<List<Client>> GetClientsAsync()
        {
            return (await QueryAsync(q => q.QueryAsync<Client>(ClientSql.GetAll))).ToList();
        }

        public async Task<List<IpAddress>> GetIpAddressesAsync()
        {
            return (await QueryAsync(q => q.QueryAsync<IpAddress>(IpAddressSql.GetAll))).ToList();
        }

        public async Task<ReturnValue> AddClientTracking(List<ClientTracking> records)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                using (var conn = await OpenConnectionAsync())
                {
                    foreach (var record in records)
                    {
                        await conn.ExecuteAsync(ClientTrackingSql.Insert, new
                        {
                            record.ClientId,
                            record.IpAddressId,
                            record.Username,
                            record.AccessPointId,
                            record.SsidId,
                            record.WlanInterfaceId,
                            record.VlanId,
                            record.BatchDate
                        });
                    }
                }
                
                sw.Stop();

                return new ReturnValue(true, null, null, sw.Elapsed);
            }
            catch (Exception ex)
            {
                return new ReturnValue(false, ex.Message);
            }
        }

        public async Task<List<ClientCountOverall>> GetOverallClientCountLastHour()
        {
            var smallestBatchDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
            return
                (await QueryAsync(
                    q =>
                        q.QueryAsync<ClientCountOverall>(ClientTrackingSql.GetOverallClientCountGreater, new {batchDate = smallestBatchDate})))
                .ToList();
        }

        public async Task<List<ClientCountOverall>> GetOverallClientCountToday()
        {
            var smallestBatchDate = DateTime.Today;
            return
                (await QueryAsync(
                    q =>
                        q.QueryAsync<ClientCountOverall>(ClientTrackingSql.GetOverallClientCountGreater, new { batchDate = smallestBatchDate })))
                .ToList();
        }

        public async Task<List<ClientCountAccessPoint>> GetAccessPointClientCountLastHour()
        {
            var smallestBatchDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
            return
                (await QueryAsync(
                    q =>
                        q.QueryAsync<ClientCountAccessPoint>(ClientTrackingSql.GetAccessPointClientCountGreater, new { batchDate = smallestBatchDate })))
                .ToList();
        }

        public async Task<List<ClientCountAccessPoint>> GetAccessPointClientCountToday()
        {
            var smallestBatchDate = DateTime.Today;
            return
                (await QueryAsync(
                    q =>
                        q.QueryAsync<ClientCountAccessPoint>(ClientTrackingSql.GetAccessPointClientCountGreater, new { batchDate = smallestBatchDate })))
                .ToList();
        }

        public async Task<List<ClientCountOverall>> GetOverallClientCountLastXHours(int hours)
        {
            var smallestBatchDate = DateTime.Now.Subtract(new TimeSpan(hours, 0, 0));
            return
                (await QueryAsync(
                    q =>
                        q.QueryAsync<ClientCountOverall>(ClientTrackingSql.GetOverallClientCountGreater, new { batchDate = smallestBatchDate })))
                .ToList();
        }

        public async Task<List<ClientCountVlan>> GetPerVlanClientCountLastXHours(int hours)
        {
            var smallestBatchDate = DateTime.Now.Subtract(new TimeSpan(hours, 0, 0));
            return
                (await QueryAsync(
                    q =>
                        q.QueryAsync<ClientCountVlan>(ClientTrackingSql.GetVlanClientCountGreater, new { batchDate = smallestBatchDate })))
                .ToList();
        }
    }
}
