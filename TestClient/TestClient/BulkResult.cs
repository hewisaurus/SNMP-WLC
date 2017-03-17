using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    public class BulkResult
    {
        public string Mib { get; set; }
        public string Value { get; set; }

        public BulkResult()
        {
            
        }

        public BulkResult(string mib, string value)
        {
            Mib = mib;
            Value = value;
        }
    }
}
