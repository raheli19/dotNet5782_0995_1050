using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class PriorityException : Exception
    {
        public PriorityException()
        {
        }

        public PriorityException(string message) : base(message)
        {
        }

        public PriorityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PriorityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}