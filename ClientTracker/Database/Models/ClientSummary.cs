using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class ClientSummary
    {
        public int Id { get; set; }
        public int BatchDateId { get; set; }
        public int SsidId { get; set; }
        public int WlanInterfaceId { get; set; }
        public int VlanId { get; set; }
        public int AccessPointId { get; set; }
        public int ClientCount { get; set; }
    }
}