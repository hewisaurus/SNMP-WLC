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
        public string MacAddress { get; set; }
        public DateTime Created { get; set; }
    }
}
