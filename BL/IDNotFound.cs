using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    internal class IDNotFound : Exception
    {
        public IDNotFound()
        {
        }

        public IDNotFound(string message) : base(message)
        {
        }

        public IDNotFound(string message, Exception innerException) : base(message, innerException)
        {
        }

        

        protected IDNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}