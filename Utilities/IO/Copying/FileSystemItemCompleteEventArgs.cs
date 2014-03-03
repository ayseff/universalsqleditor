using System;

namespace Utilities.IO.Copying
{
    /// <summary>
    /// Event arguments when a file has finished copying.
    /// </summary>
    [Serializable]
    public class FileSystemItemCompleteEventArgs : System.EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the FileSystemItemCompleteEventArgs class.
        /// </summary>
        /// <param name="sourceFullName">Source file name being copied.</param>
        /// <param name="targetFullName">Target file name.</param>
        /// <param name="size">Size of the file being copied.</param>
        /// <param name="elapsedTime">Time elapsed to copy the file.</param>
        public FileSystemItemCompleteEventArgs(string sourceFullName, string targetFullName, long size, TimeSpan elapsedTime)
        {
            TargetFullName = targetFullName;
            Size = size;
            SourceFullName = sourceFullName;
            ElapsedTime = elapsedTime;
        }

        /// <summary>
        /// Gets the total size (in bytes) of the item being copied.
        /// </summary>
        /// <value> The total file size. </value>
        public long Size { get; private set; }

        /// <summary>
        /// Gets the source full name currently being copied.
        /// </summary>
        public string SourceFullName { get; private set; }

        /// <summary>
        /// Gets the target full name currently being copied.
        /// </summary>
        public string TargetFullName { get; private set; }

        /// <summary>
        /// Gets amount of time elapsed to complete the copy process.
        /// </summary>
        public TimeSpan ElapsedTime { get; private set; }
    }
}