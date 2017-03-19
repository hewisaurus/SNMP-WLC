using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Connection
{
    public class MySqlConnectionFactory : IConnectionFactory
    {
        private readonly string _connectionString;

        public MySqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbConnection Create()
        {
            var factory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
            var conn = factory.CreateConnection();
            conn.ConnectionString = _connectionString;

            return conn;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
