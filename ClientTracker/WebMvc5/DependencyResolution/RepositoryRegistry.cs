using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;
using Database;
using Database.Connection;
using StructureMap;
using WebMvc5.Helpers.Charts;

namespace WebMvc5.DependencyResolution
{
    public class RepositoryRegistry : Registry
    {
        public RepositoryRegistry()
        {
            For<IConnectionFactory>()
                .Use<MySqlConnectionFactory>()
                .Ctor<string>("connectionString")
                .Is(Connection.DatabaseConnectionMysql);

            For<IDatabaseRepository>().Use<DatabaseRepository>();

            For<IChartRepository>().Use<ChartRepository>();
        }
    }
}