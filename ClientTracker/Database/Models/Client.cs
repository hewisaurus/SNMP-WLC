using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string MacAddress { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastSeen { get; set; }
    }
}
