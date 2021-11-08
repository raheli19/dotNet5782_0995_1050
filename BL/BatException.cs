using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class BatException : Exception
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
}