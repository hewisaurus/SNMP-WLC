using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Glimpse.Ado.AlternateType;
using MySql.Data.MySqlClient;

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
            return new GlimpseDbConnection(new MySqlConnection(_connectionString));
            //var conn = MySqlClientFactory.Instance.CreateConnection();
            //conn.ConnectionString = _connectionString;
            //var factory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
            //var factory = DbProviderFactories.GetFactory("MySqlClientFactory");
            //var factory = MySqlClientFactory.
            //var conn = factory.CreateConnection();
            //conn.ConnectionString = _connectionString;

            //return conn;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
