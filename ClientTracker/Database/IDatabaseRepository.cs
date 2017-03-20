using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Database.Models;

namespace Database
{
    public interface IDatabaseRepository
    {
        void LogLine(string line);
        
        Task<List<AccessPointModel>> GetApModelsAsync();
        Task<List<AccessPoint>> GetAccessPointsAsync();
        Task<List<Ssid>> GetSsidsAsync();
        Task<List<Vlan>> GetVlansAsync();
        Task<List<WlanInterface>> GetWlanInterfacesAsync();
        Task<List<Client>> GetClientsAsync();
        Task<List<IpAddress>> GetIpAddressesAsync();

        Task<ReturnValue> UpdateAccessPointModels(List<string> models);
        Task<ReturnValue> UpdateAccessPoints(List<AccessPoint> accessPoints);
        Task<ReturnValue> UpdateSsids(List<string> ssids);
        Task<ReturnValue> UpdateVlans(List<string> vlans);
        Task<ReturnValue> UpdateWlanInterfaces(List<string> interfaces);
        Task<ReturnValue> UpdateClients(List<string> clients);
        Task<ReturnValue> UpdateIpAddresses(List<string> addresses);
        Task<ReturnValue> AddClientTracking(List<ClientTracking> records);
    }
}
