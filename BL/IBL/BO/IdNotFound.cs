using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    internal class IdNotFound : Exception
    {
        public IdNotFound()
        {
        }

        public IdNotFound(string message) : base(message)
        {
        }

        public IdNotFound(string message, Exception innerException) : base(message, innerException)
        {
        }

       

        protected IdNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}