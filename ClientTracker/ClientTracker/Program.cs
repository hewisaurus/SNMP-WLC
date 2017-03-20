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
using Hangfire.Storage;
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
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute(){Attempts = 0});
            var options = new BackgroundJobServerOptions()
            {
                WorkerCount = 2
            };
            using (new BackgroundJobServer(options))
            {
                using (var connection = JobStorage.Current.GetConnection())
                {
                    foreach (var recurringJob in connection.GetRecurringJobs())
                    {
                        RecurringJob.RemoveIfExists(recurringJob.Id);
                    }
                }

                Thread.Sleep(5000);
                //await Task.Delay(5000);

                var app = container.GetInstance<ClientTracker>();
                app.Run();

                // This makes sure the database update is a recurring task
                RecurringJob.AddOrUpdate("UpdateDatabase", () => app.UpdateDatabase(), "*/1 * * * *");

                Console.WriteLine("Hangfire Server started. Press ENTER to exit...");
                Console.ReadLine();
                Console.ReadLine();
            }

            
            

            //Console.ReadLine();




            //var client = new BackgroundJobClient();

            //client.Enqueue(() => Console.WriteLine("Easy!"));
        }
    }
}
