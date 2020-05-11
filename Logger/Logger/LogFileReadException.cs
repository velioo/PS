using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    class LogFileReadException : Exception
    {
        public LogFileReadException() : base() { }
        public LogFileReadException(string message) : base(message) { }
        public LogFileReadException(string message, System.Exception inner) : base(message, inner) { }
        protected LogFileReadException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
