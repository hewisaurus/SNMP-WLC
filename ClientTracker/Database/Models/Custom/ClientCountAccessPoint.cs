﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Custom
{
    public class ClientCountAccessPoint
    {
        public DateTime BatchDate { get; set; }
        public long Clients { get; set; }
        public string AccessPoint { get; set; }
    }
}