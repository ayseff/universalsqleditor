using System;
using System.ComponentModel;

namespace Utilities.IO.Copying
{
    /// <summary>
    /// Event arguments when a file is about to be copied.
    /// </summary>
    [Serializable]
    public class FileSystemItemStartedEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the FileCopyProgressChangedEventArgs class.
        /// </summary>
        /// <param name="sourceFullName">Source file name being copied.</param>
        /// <param name="targetFullName">Target file name.</param>
        /// <param name="fileSize">Size of the file being copied.</param>
        public FileSystemItemStartedEventArgs(string sourceFullName, string targetFullName, long fileSize)
        {
            TargetFullName = targetFullName;
            Size = fileSize;
            SourceFullName = sourceFullName;
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
    }
}