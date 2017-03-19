using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Sql
{
    public class AccessPointSql
    {
        public const string GetAll = "SELECT * FROM AccessPoint AP JOIN AccessPointModel APM ON AP.ModelId = APM.Id";
        public const string GetById = "SELECT * FROM AccessPoint AP JOIN AccessPointModel APM ON AP.ModelId = APM.Id WHERE AP.Id = @id";
        public const string GetByEthernetMac = "SELECT * FROM AccessPoint AP JOIN AccessPointModel APM ON AP.ModelId = APM.Id WHERE AP.EthernetMacAddress = @ethernetMacAddress";
        public const string GetIdByEthernetMac = "SELECT Id FROM AccessPoint WHERE EthernetMacAddress = @ethernetMacAddress";

        public const string Insert =
            "INSERT INTO AccessPoint(Name,ModelId,Location,EthernetMacAddress,BaseRadioMacAddress,IpAddress)" +
            "VALUES(@name,@modelId,@location,@ethernetMacAddress,@baseRadioMacAddress,@ipAddress)";
        public const string Update =
            "UPDATE AccessPoint SET Name = @name, Location = @location, IpAddress = @ipAddress, LastSeen = @lastSeen WHERE Id = @id";
    }
}
