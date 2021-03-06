﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Database.Models;
using Database.Models.Custom;

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

        Task<Vlan> GetVlanAsync(int id);
        Task<AccessPoint> GetAccessPointAsync(int id);

        Task<ReturnValue> UpdateAccessPointModels(List<string> models);
        Task<ReturnValue> UpdateAccessPoints(List<AccessPoint> accessPoints);
        Task<ReturnValue> UpdateSsids(List<string> ssids);
        Task<ReturnValue> UpdateVlans(List<string> vlans);
        Task<ReturnValue> UpdateWlanInterfaces(List<string> interfaces);
        Task<ReturnValue> UpdateClients(List<string> clients);
        Task<ReturnValue> UpdateIpAddresses(List<string> addresses);
        Task<ReturnValue> AddClientTracking(List<ClientTracking> records);

        Task<List<ClientCountOverall>> GetOverallClientCountLastHour();
        Task<List<ClientCountOverall>> GetOverallClientCountToday();
        Task<List<ClientCountAccessPoint>> GetAccessPointClientCountLastHour();
        Task<List<ClientCountAccessPoint>> GetAccessPointClientCountToday();
        Task<List<ClientCountOverall>> GetOverallClientCountLastXHours(int hours);
        Task<List<ClientCountVlan>> GetPerVlanClientCountLastXHours(int hours);
        Task<List<ClientCountAccessPoint>> GetPerAccessClientCountLastXHours(int hours);

        // Summary methods
        //Task<List<BatchDate>> GetDatesInRange()

        Task<List<ClientSummary>> GetFullSummary(DateTime startDate, DateTime endDate);
        Task<List<DateClientCount>> GetClientCountSummary(DateTime startDate, DateTime endDate, int apId = 0, int vlanId = 0);
    }
}
