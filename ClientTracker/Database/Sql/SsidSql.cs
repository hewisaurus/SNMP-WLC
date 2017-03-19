using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Sql
{
    public class SsidSql
    {
        public const string GetAll = "SELECT * FROM Ssid";
        public const string GetAllByName = "SELECT * FROM Ssid WHERE Value IN @values";
        public const string GetByName = "SELECT * FROM Ssid WHERE Value = @value";
        public const string GetById = "SELECT * FROM Ssid WHERE Id = @id";
        public const string Insert = "INSERT INTO Ssid(Value) VALUES(@value)";
        public const string Update = "UPDATE Ssid SET Value = @value, LastSeen = @lastSeen WHERE Id = @id";
    }
}