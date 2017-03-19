using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ReturnValue
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public TimeSpan TimeTaken { get; set; }

        public ReturnValue()
        {
            Initialise(false, "", null, new TimeSpan());
        }

        public ReturnValue(bool success, string message)
        {
            Initialise(success, message, null, new TimeSpan());
        }

        public ReturnValue(ReturnValue returnvalue)
        {
            Initialise(returnvalue.Success, returnvalue.Message, returnvalue.Data, returnvalue.TimeTaken);
        }

        private void Initialise(bool success, string message, object data, TimeSpan timeTaken)
        {
            Success = success;
            Message = message;
            Data = data;
            TimeTaken = timeTaken;
        }
        

        public ReturnValue(bool success, string message, object data)
        {
            Initialise(success, message, data, new TimeSpan());
        }

        public ReturnValue(bool success, string message, object data, TimeSpan timeTaken)
        {
            Initialise(success, message, data, timeTaken);
        }
    }
}
