using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Sql
{
    public class VlanSql
    {
        public const string GetAll = "SELECT * FROM Vlan";
        public const string GetAllByName = "SELECT * FROM Vlan WHERE Value IN @values";
        public const string GetByName = "SELECT * FROM Vlan WHERE Value = @value";
        public const string GetById = "SELECT * FROM Vlan WHERE Id = @id";
        public const string Insert = "INSERT INTO Vlan(Value) VALUES(@value)";
        public const string Update = "UPDATE Vlan SET Value = @value, LastSeen = @lastSeen WHERE Id = @id";
    }
}