using System;
using System.ComponentModel;

namespace Utilities.IO.Copying
{
    [Serializable]
    public class DirectoryCompleteEventArgs : CancelEventArgs
    {
        private string _directoryName;
        private long _totalDirectorySize;

        /// <summary>
        ///   Initializes a new instance of the DirectoryCopyCompleteEventArgs class.
        /// </summary>
        /// <param name="totalDirectorySize"> The total directory size, in bytes. </param>
        /// <param name="totalBytesTransferred"> The total bytes transferred so far. </param>
        /// <param name="directoryName"> Directory name being copied. </param>
        public DirectoryCompleteEventArgs(long totalDirectorySize, long totalBytesTransferred, string directoryName)
        {
            _totalDirectorySize = totalDirectorySize;
            _directoryName = directoryName;
        }

        /// <summary>
        ///   Gets the total directory size in bytes.
        /// </summary>
        /// <value> The total directory size in bytes. </value>
        public long TotalDirectorySize
        {
            get { return _totalDirectorySize; }
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