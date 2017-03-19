using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Sql
{
    public class AccessPointModelSql
    {
        public const string GetAll = "SELECT * FROM AccessPointModel";
        public const string GetByName = "SELECT * FROM AccessPointModel WHERE Name = @name";
        public const string GetById = "SELECT * FROM AccessPointModel WHERE Id = @id";
        public const string Insert = "INSERT INTO AccessPointModel(Name) VALUES(@name);";
    }
}