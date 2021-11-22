using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class WeightException : Exception
    {
        public WeightException()
        {
        }

        public WeightException(string message) : base(message)
        {
        }

        public WeightException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WeightException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}