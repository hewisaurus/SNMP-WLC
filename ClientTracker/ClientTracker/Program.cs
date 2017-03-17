﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientTracker.DI;
using Common;
using Hangfire;
using Hangfire.MySql;
using Hangfire.StructureMap;
using StructureMap;

namespace ClientTracker
{
    class Program
    {
        static void Main(string[] args)
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

            //BackgroundJob.Enqueue(() => app.UpdateDatabase());
            //BackgroundJob.Schedule(() => app.UpdateDatabase(), TimeSpan.FromSeconds(30));
            RecurringJob.AddOrUpdate("UpdateDatabase", () => app.UpdateDatabase(), "*/1 * * * *");

            //app.UpdateDatabase();

            //Console.ReadLine();




            //var client = new BackgroundJobClient();

            //client.Enqueue(() => Console.WriteLine("Easy!"));



        }
    }
}
