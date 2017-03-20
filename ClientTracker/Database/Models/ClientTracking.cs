using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class ClientTracking
    {
        public long Id { get; set; }
        public int ClientId { get; set; }
        public int IpAddressId { get; set; }
        public string Username { get; set; }
        public int AccessPointId { get; set; }
        public int SsidId { get; set; }
        public int VlanId { get; set; }
        public int WlanInterfaceId { get; set; }
        public DateTime BatchDate { get; set; }
        public DateTime Created { get; set; }
    }
}
