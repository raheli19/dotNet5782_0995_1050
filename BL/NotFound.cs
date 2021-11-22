﻿using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    public class NotFound : Exception
    {
        public NotFound()
        {
        }

        public NotFound(string message) : base(message)
        {
        }

        public NotFound(string message, Exception innerException) : base(message, innerException)
        {
        }


        protected NotFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}