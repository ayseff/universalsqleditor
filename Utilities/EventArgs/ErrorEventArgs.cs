#region

using System;

#endregion

namespace Utilities.EventArgs
{
    public class ErrorEventArgs : System.EventArgs
    {
        private readonly string _errorMessage;
        private readonly Exception _exception;

        public ErrorEventArgs(string errorMessage, Exception e)
        {
            if (errorMessage == null)
            {
                throw new ArgumentNullException("errorMessage", "Error message cannot be null.");
            }
            _errorMessage = errorMessage;
            _exception = e;
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
        }

        public Exception Exception
        {
            get { return _exception; }
        }

        public override string ToString()
        {
            return ErrorMessage;
        }
    }
}