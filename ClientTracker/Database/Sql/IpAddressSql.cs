using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Sql
{
    public class IpAddressSql
    {
        public const string GetAll = "SELECT * FROM IpAddress";
        public const string GetAllByAddress = "SELECT * FROM IpAddress WHERE Value IN @values";
        public const string GetByAddress = "SELECT * FROM IpAddress WHERE Value = @value";
        public const string GetById = "SELECT * FROM IpAddress WHERE Id = @id";
        public const string Insert = "INSERT INTO IpAddress(Value) VALUES(@value)";
        public const string Update = "UPDATE IpAddress SET LastSeen = @lastSeen WHERE Id = @id";
    }
}
