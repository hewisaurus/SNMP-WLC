﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class Vlan
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastSeen { get; set; }
    }
}
