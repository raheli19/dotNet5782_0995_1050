using System;
using System.Runtime.Serialization;

namespace BO
{
    #region Alreadyexists
    [Serializable]
    internal class AlreadyExist : Exception
    {
        public AlreadyExist()
        {
        }

        public AlreadyExist(string message) : base(message)
        {
        }

        public AlreadyExist(string message, Exception innerException) : base(message, innerException)
        {
        }



        protected AlreadyExist(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    #endregion

    #region CannotAdd
    [Serializable]
        internal class CannotAdd : Exception
        {
            public CannotAdd()
            {
            }

            public CannotAdd(string message) : base(message)
            {
            }

            public CannotAdd(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected CannotAdd(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    #endregion

    #region CannotUpdate
    [Serializable]
        internal class CannotUpdate : Exception
        {
            public CannotUpdate()
            {
            }

            public CannotUpdate(string message) : base(message)
            {
            }

            public CannotUpdate(string message, Exception innerException) : base(message, innerException)
            {
            }


            protected CannotUpdate(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }
    #endregion
   
    #region DroneChargedExcept
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
    #endregion

    #region IdNotFound
    [Serializable]
    public class IDNotFound : Exception
    {
        public IDNotFound()
        {
        }

        public IDNotFound(string message) : base(message)
        {
        }

        public IDNotFound(string message, Exception innerException) : base(message, innerException)
        {
        }



        protected IDNotFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    #endregion

    #region InputNotValid
    [Serializable]
    public class InputNotValid : Exception
    {
        public InputNotValid()
        {
        }

        public InputNotValid(string message) : base(message)
        {
        }

        public InputNotValid(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InputNotValid(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    #endregion

    #region NotAvailable
    [Serializable]
    internal class NotAvailable : Exception
    {
        public NotAvailable()
        {
        }

        public NotAvailable(string message) : base(message)
        {
        }

        protected NotAvailable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    #endregion

    #region NotEnoughBattery
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
    #endregion

    #region NotFound
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
    #endregion

    #region Priority
    [Serializable]
    internal class PriorityException : Exception
    {
        public PriorityException()
        {
        }

        public PriorityException(string message) : base(message)
        {
        }

        public PriorityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PriorityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    internal class StatusException : Exception
    {
        public StatusException()
        {
        }

        public StatusException(string message) : base(message)
        {
        }

        public StatusException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StatusException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    #endregion

    #region UpdateError
    [Serializable]
    internal class WrongDetailsUpdateException : Exception
    {
        public WrongDetailsUpdateException()
        {
        }

        public WrongDetailsUpdateException(string message) : base(message)
        {
        }

        public WrongDetailsUpdateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongDetailsUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    #endregion
}







