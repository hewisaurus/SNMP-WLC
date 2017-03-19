using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Sql
{
    public class ClientSql
    {
        public const string GetAll = "SELECT * FROM Client";
        public const string GetAllByAddress = "SELECT * FROM Client WHERE MacAddress IN @macAddresses";
        public const string GetByAddress = "SELECT * FROM Client WHERE MacAddress = @macAddress";
        public const string GetById = "SELECT * FROM Client WHERE Id = @id";
        public const string Insert = "INSERT INTO Client(MacAddress) VALUES(@macAddress)";
        public const string Update = "UPDATE Client SET LastSeen = @lastSeen WHERE Id = @id";
    }
}
