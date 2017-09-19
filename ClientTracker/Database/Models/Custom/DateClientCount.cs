using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Custom
{
    public class DateClientCount
    {
        public DateTime Date { get; set; }
        public int ClientCount { get; set; }

        public DateClientCount()
        {
            
        }

        public DateClientCount(DateTime date, int clientCount)
        {
            Date = date;
            ClientCount = clientCount;
        }
    }
}