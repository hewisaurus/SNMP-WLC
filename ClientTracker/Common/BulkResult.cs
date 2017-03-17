using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
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
