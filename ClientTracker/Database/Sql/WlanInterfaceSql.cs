using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Sql
{
    public class WlanInterfaceSql
    {
        public const string GetAll = "SELECT * FROM WlanInterface";
        public const string GetAllByName = "SELECT * FROM WlanInterface WHERE Value IN @values";
        public const string GetByName = "SELECT * FROM WlanInterface WHERE Value = @value";
        public const string GetById = "SELECT * FROM WlanInterface WHERE Id = @id";
        public const string Insert = "INSERT INTO WlanInterface(Value) VALUES(@value)";
        public const string Update = "UPDATE WlanInterface SET Value = @value, LastSeen = @lastSeen WHERE Id = @id";
    }
}
