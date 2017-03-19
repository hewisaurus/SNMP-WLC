using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Database;
using Database.Connection;
using StructureMap;

namespace ClientTracker.DI
{
    public class AppRegistry : Registry
    {
        public AppRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });

            For<IConnectionFactory>()
                .Use<MySqlConnectionFactory>()
                .Ctor<string>("connectionString")
                .Is(Connection.DatabaseConnectionMysql);

            For<IDatabaseRepository>().Use<DatabaseRepository>();
        }
    }
}
