﻿using System;
using System.Runtime.Serialization;

namespace IBL.BO
{
    [Serializable]
    internal class NotAvailable : Exception
    {
        public NotAvailable()
        {
        }

        public NotAvailable(string message) : base(message)
        {
        }

        public NotAvailable(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotAvailable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}