using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace ClientTracker
{
    public class ClientTracker
    {
        private readonly IConsoleWriter _writer;
        private readonly IDatabaseRepository _database;

        public ClientTracker(IConsoleWriter writer, IDatabaseRepository database)
        {
            _writer = writer;
            _database = database;
        }

        public void Run()
        {
            _writer.WriteLine("Application running!");
        }

        public void UpdateDatabase()
        {
            _database.LogLine("test!");
        }
    }
}
