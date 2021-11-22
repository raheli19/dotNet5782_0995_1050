using System;
using System.Runtime.Serialization;

namespace IBL.BO
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

    [Serializable]
    public class BatException : Exception
    {
        public BatException()
        {
        }

        public BatException(string message) : base(message)
        {
        }

        public BatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class IDException : Exception
    {
        public IDException()
        {
        }

        public IDException(string message) : base(message)
        {
        }

        public IDException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected IDException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }


    [Serializable]
    public class LatException : Exception
    {
        public LatException()
        {
        }

        public LatException(string message) : base(message)
        {
        }

        public LatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }


    [Serializable]
    public class LongException : Exception
    {
        public LongException()
        {
        }

        public LongException(string message) : base(message)
        {
        }

        public LongException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected LongException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }


    [Serializable]
    public class PhoneException : Exception
    {
        public PhoneException()
        {
        }

        public PhoneException(string message) : base(message)
        {
        }

        public PhoneException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PhoneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}