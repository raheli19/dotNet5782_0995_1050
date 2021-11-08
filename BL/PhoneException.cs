using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class PhoneException : Exception
    {
        public PhoneException()
        {
        }

        public PhoneException(string message) : base(message)
        {
        }

        public PhoneException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PhoneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}