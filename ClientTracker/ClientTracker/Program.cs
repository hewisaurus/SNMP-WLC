using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClientTracker.DI;
using Common;
using Hangfire;
using Hangfire.Logging;
using Hangfire.Logging.LogProviders;
using Hangfire.MySql;
using Hangfire.StructureMap;
using StructureMap;

namespace ClientTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();
        }

        static async void Start()
        {
            var container = Container.For<AppRegistry>();

            GlobalConfiguration.Configuration.UseStructureMapActivator(container);
            GlobalConfiguration.Configuration.UseColouredConsoleLogProvider().UseStorage(new MySqlStorage(Connection.DatabaseConnectionHangfire));

            using (new BackgroundJobServer())
            {
                Console.WriteLine("Hangfire Server started. Press ENTER to exit...");
                Console.ReadLine();
                Console.ReadLine();
            }

            var app = container.GetInstance<ClientTracker>();
            app.Run();

            // This makes sure the database update is a recurring task
            RecurringJob.AddOrUpdate("UpdateDatabase", () => app.UpdateDatabase(), "*/1 * * * *");
            

            //Console.ReadLine();




            //var client = new BackgroundJobClient();

            //client.Enqueue(() => Console.WriteLine("Easy!"));
        }
    }
}
