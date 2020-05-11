using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logger
{
    public class LogFileWriteException : Exception
    {
        public LogFileWriteException() : base() { }
        public LogFileWriteException(string message) : base(message) { }
        public LogFileWriteException(string message, System.Exception inner) : base(message, inner) { }
        protected LogFileWriteException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
