using System;
using System.ComponentModel;

namespace Utilities.IO.Copying
{
    [Serializable]
    public class DirectoryProgressChangedEventArgs : CancelEventArgs
    {
        private readonly string _directoryName;
        private readonly long _directorySize;
        private readonly long _totalBytesTransferred;

        /// <summary>
        ///   Initializes a new instance of the DirectoryCopyProgressChangedEventArgs class.
        /// </summary>
        /// <param name="directorySize"> The total directory size, in bytes. </param>
        /// <param name="bytesTransferred"> The total bytes transferred so far. </param>
        /// <param name="directoryName"> Directory name being copied. </param>
        public DirectoryProgressChangedEventArgs(string directoryName, long directorySize, long bytesTransferred)
        {
            _directorySize = directorySize;
            _totalBytesTransferred = bytesTransferred;
            _directoryName = directoryName;
        }

        /// <summary>
        ///   Gets the total directory size in bytes.
        /// </summary>
        /// <value> The total directory size in bytes. </value>
        public long DirectorySize
        {
            get { return _directorySize; }
        }

        /// <summary>
        ///   Gets the total number of bytes transferred so far.
        /// </summary>
        /// <value> The total bytes transferred. </value>
        public long BytesTransferred
        {
            get { return _totalBytesTransferred; }
        }

        /// <summary>
        ///   Gets the directory name currently being copied.
        /// </summary>
        public string DirectoryPath
        {
            get { return _directoryName; }
        }
    }
}