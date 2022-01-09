using System;
using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    internal class XmlFileLoadException : Exception
    {
        public XmlFileLoadException()
        {
        }

        public XmlFileLoadException(string message) : base(message)
        {
        }

        public XmlFileLoadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected XmlFileLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}