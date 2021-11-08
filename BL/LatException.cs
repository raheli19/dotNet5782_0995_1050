using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class LatException : Exception
    {
        public LatException()
        {
        }

        public LatException(string message) : base(message)
        {
        }

        public LatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}