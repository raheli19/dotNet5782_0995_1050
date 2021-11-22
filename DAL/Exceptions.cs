using System;
using System.Runtime.Serialization;

namespace IDAL.DO
{
    [Serializable]
    public class StationException : Exception
    {
        public StationException()
        {
        }

        public StationException(string message) : base(message)
        {
        }

        public StationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }


    [Serializable]
    public class ParcelException : Exception
    {
        public ParcelException()
        {
        }

        public ParcelException(string message) : base(message)
        {
        }

        public ParcelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ParcelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }


    [Serializable]
    public class DroneException : Exception
    {
        public DroneException()
        {
        }

        public DroneException(string message) : base(message)
        {
        }

        public DroneException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }



    [Serializable]
    public class ClientException : Exception
    {
        public ClientException()
        {
        }

        public ClientException(string message) : base(message)
        {
        }

        public ClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}