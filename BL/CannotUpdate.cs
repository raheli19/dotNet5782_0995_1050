using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    internal class CannotUpdate : Exception
    {
        public CannotUpdate()
        {
        }

        public CannotUpdate(string message) : base(message)
        {
        }

        public CannotUpdate(string message, Exception innerException) : base(message, innerException)
        {
        }


        protected CannotUpdate(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}