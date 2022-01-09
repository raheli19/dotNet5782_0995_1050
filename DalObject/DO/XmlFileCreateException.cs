using System;
using System.Runtime.Serialization;

namespace DO
{
    [Serializable]
    internal class XmlFileCreateException : Exception
    {
        public XmlFileCreateException()
        {
        }

        public XmlFileCreateException(string message) : base(message)
        {
        }

        public XmlFileCreateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected XmlFileCreateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}