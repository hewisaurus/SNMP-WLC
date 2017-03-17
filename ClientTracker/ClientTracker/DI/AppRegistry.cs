using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;
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

            For<IDatabaseRepository>().Use<DatabaseRepository>();
        }
    }
}
