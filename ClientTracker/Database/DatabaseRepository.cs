using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Dapper;
using Database.Connection;
using Database.Models;
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
                using (var conn = await OpenConnectionAsync())
                {
                    foreach (var model in models)
                    {
                        // Check if this model exists. If it doesn't, then insert it
                        var dbModel =
                            (await conn.QueryAsync<AccessPointModel>(AccessPointModelSql.GetByName, new { Name = model }))
                            .SingleOrDefault();
                        if (dbModel == null)
                        {
                            await conn.ExecuteAsync(AccessPointModelSql.Insert, new { name = model });
                        }
                    }
                }

                return new ReturnValue(true, null);
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

        public async Task<ReturnValue> UpdateAccessPoints(List<AccessPoint> accessPoints)
        {
            try
            {
                using (var conn = await OpenConnectionAsync())
                {
                    foreach (var accessPoint in accessPoints)
                    {
                        // Check if this AP exists. If it doesn't, then insert it
                        var dbModel =
                            (await conn.QueryAsync<int>(AccessPointSql.GetIdByEthernetMac, new { ethernetMacAddress = accessPoint.EthernetMacAddress.Replace(" ","").ToUpper() }))
                            .SingleOrDefault();
                        if (dbModel <= 0)
                        {
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
                                id = dbModel,
                                lastSeen = DateTime.Now
                            });
                        }
                    }
                }

                return new ReturnValue(true, null);
            }
            catch (Exception ex)
            {
                return new ReturnValue(false, ex.Message);
            }
        }
    }
}
