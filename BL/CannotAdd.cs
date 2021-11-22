using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    internal class CannotAdd : Exception
    {
        public CannotAdd()
        {
        }

        public CannotAdd(string message) : base(message)
        {
        }

        public CannotAdd(string message, Exception innerException) : base(message, innerException)
        {
        }
        protected CannotAdd(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}