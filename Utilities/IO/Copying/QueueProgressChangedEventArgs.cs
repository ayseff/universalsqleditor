using System;

namespace Utilities.IO.Copying
{
    /// <summary>
    /// Describes queue.
    /// </summary>
    [Serializable]
    public class QueueProgressChangedEventArgs : System.EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueProgressChangedEventArgs"/> class.
        /// </summary>
        /// <param name="size"> Total queue size. </param>
        /// <param name="bytesTransferred"> Total bytes transferred so far. </param>
        /// <param name="itemsTotal"> Total number of items in the queue. </param>
        /// <param name="itemsComplete"> Total number of items complete. </param>
        public QueueProgressChangedEventArgs(long size, long bytesTransferred, int itemsTotal, int itemsComplete)
        {
            Size = size;
            BytesTransferred = bytesTransferred;
            ItemsTotal = itemsTotal;
            ItemsComplete = itemsComplete;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueProgressChangedEventArgs"/> class.
        /// </summary>
        /// <param name="queueInfo"> QueueInfo from which to get info. </param>
        public QueueProgressChangedEventArgs(QueueInfo queueInfo)
        {
            Size = queueInfo.QueueByteSize;
            BytesTransferred = queueInfo.QueueBytesTransferred;
            ItemsTotal = queueInfo.ItemCount;
            ItemsComplete = queueInfo.ItemsCompleteCount;
        }

        /// <summary>
        ///   Gets the total queue size in bytes.
        /// </summary>
        /// <value> The total queue size. </value>
        public long Size { get; private set; }

        /// <summary>
        ///   Gets the number of items in the queue.
        /// </summary>
        public int ItemsTotal { get; private set; }

        /// <summary>
        ///   Gets the number of completed items in the queue.
        /// </summary>
        public int ItemsComplete { get; private set; }

        /// <summary>
        ///   Gets the total number of bytes transferred so far.
        /// </summary>
        /// <value> The total bytes transferred. </value>
        public long BytesTransferred { get; private set; }
    }
}