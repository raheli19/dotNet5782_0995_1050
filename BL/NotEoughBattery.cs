using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    internal class NotEoughBattery : Exception
    {
        public NotEoughBattery()
        {
        }

        public NotEoughBattery(string message) : base(message)
        {
        }

        public NotEoughBattery(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotEoughBattery(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}