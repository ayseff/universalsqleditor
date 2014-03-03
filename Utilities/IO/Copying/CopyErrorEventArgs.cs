using System;
using Utilities.EventArgs;

namespace Utilities.IO.Copying
{
    /// <summary>
    /// Represents an event that an error copying has occured.
    /// </summary>
    public class CopyErrorEventArgs : ErrorEventArgs
    {
        /// <summary>
        /// Copy pair which caused the error
        /// </summary>
        public CopyPair CopyPair { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="copyPair"></param>
        /// <param name="e"></param>
        public CopyErrorEventArgs(string errorMessage, CopyPair copyPair, Exception e)
            : base(errorMessage, e)
        {
            CopyPair = copyPair;
        }

        public override string ToString()
        {
            return ErrorMessage;
        }
    }
}
