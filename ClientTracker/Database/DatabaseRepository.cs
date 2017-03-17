using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class DatabaseRepository : IDatabaseRepository
    {
        public void LogLine(string line)
        {
            Console.WriteLine(line);
        }
    }
}
