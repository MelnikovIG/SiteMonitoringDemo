using System;

namespace SiteMonitoring.DataAccess.Exceptions
{
    public class EntityNotExistException : Exception
    {
        public EntityNotExistException()
        {
        }

        public EntityNotExistException(string message)
            : base(message)
        {
        }

        public EntityNotExistException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
