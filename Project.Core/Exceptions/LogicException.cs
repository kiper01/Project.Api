using Project.Core.Enum;
using System;
using System.Runtime.Serialization;

namespace Project.Core.Exceptions
{
    public class LogicException : Exception
    {
        public ExceptionMessage? MessageType { get; }

        public LogicException()
        {

        }

        public LogicException(ExceptionMessage message)
        {
            MessageType = message;
        }

        public LogicException(string message) : base(message)
        {
        }

        protected LogicException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
