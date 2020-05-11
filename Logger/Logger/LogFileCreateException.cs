using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class LogFileCreateException : Exception
    {
        public LogFileCreateException() : base() { }
        public LogFileCreateException(string message) : base(message) { }
        public LogFileCreateException(string message, System.Exception inner) : base(message, inner) { }
        protected LogFileCreateException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
