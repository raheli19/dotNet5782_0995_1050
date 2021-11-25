using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    internal class WrongDetailsUpdateException : Exception
    {
        public WrongDetailsUpdateException()
        {
        }

        public WrongDetailsUpdateException(string message) : base(message)
        {
        }

        public WrongDetailsUpdateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongDetailsUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}