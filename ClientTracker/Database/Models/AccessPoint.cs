using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class AccessPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModelId { get; set; }
        public string Location { get; set; }
        public string EthernetMacAddress { get; set; }
        public string BaseRadioMacAddress { get; set; }
        public string IpAddress { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime LastSeen { get; set; }

        // Dapper
        public AccessPointModel Model { get; set; }
    }
}
