using System;

namespace Utilities.IO.Copying
{
    /// <summary>
    /// Describes file copy progress.
    /// </summary>
    [Serializable]
    public class FileProgressChangedEventArgs : System.EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the FileProgressChangedEventArgs class.
        /// </summary>
        /// <param name="fileName">File name being copied.</param>
        /// <param name="fileSize">File size (in bytes) being copied</param>
        /// <param name="bytesTransferred">Number of bytes transferred.</param>
        /// <param name="elapsedTime">Time elapsed.</param>
        public FileProgressChangedEventArgs(string fileName, long fileSize, long bytesTransferred, TimeSpan elapsedTime)
        {
            Size = fileSize;
            BytesTransferred = bytesTransferred;
            FullName = fileName;
            ElapsedTime = elapsedTime;
        }

        /// <summary>
        ///   Gets the total file size in bytes of the file being copied.
        /// </summary>
        /// <value> The total file size in bytes. </value>
        public long Size { get; private set; }

        /// <summary>
        ///   Gets the total number of bytes transferred so far.
        /// </summary>
        /// <value> The total bytes transferred. </value>
        public long BytesTransferred { get; private set; }

        /// <summary>
        ///   Gets the file name currently being copied.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        ///   Amount of time elapsed while copying <code>BytesTransferred</code> bytes.
        /// </summary>
        public TimeSpan ElapsedTime { get; private set; }
    }
}