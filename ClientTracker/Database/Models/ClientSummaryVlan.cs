using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class ClientSummaryVlan
    {
        public int Id { get; set; }
        public int BatchDate { get; set; }
        public int VlanId { get; set; }
        public int ClientCount { get; set; }
    }
}