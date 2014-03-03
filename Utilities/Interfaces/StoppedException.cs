using System;

namespace Utilities.Interfaces
{
    public class StoppedException : Exception
    {
        public StoppedException()
        {
        }

        public StoppedException(string message) : base(message)
        {
        }

        public StoppedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}