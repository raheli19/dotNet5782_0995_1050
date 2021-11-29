using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    public class InputNotValid : Exception
    {
        public InputNotValid()
        {
        }

        public InputNotValid(string message) : base(message)
        {
        }

        public InputNotValid(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InputNotValid(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}