using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class DroneChargedException : Exception
    {
        public DroneChargedException()
        {
        }

        public DroneChargedException(string message) : base(message)
        {
        }

        public DroneChargedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneChargedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}