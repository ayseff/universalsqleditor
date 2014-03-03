using System;
using System.ComponentModel;

namespace Utilities.IO.Copying
{
    /// <summary>
    /// Describes queue when it starts copying.
    /// </summary>
    [Serializable]
    public class QueueStartedEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueueStartedEventArgs"/> class.
        /// </summary>
        /// <param name="size"> Total queue size. </param>
        /// <param name="itemsTotal"> Total number of items in the queue. </param>
        public QueueStartedEventArgs(long size, int itemsTotal)
        {
            Size = size;
            ItemsTotal = itemsTotal;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueStartedEventArgs"/> class.
        /// </summary>
        /// <param name="queueInfo"> QueueInfo from which to get info. </param>
        public QueueStartedEventArgs(QueueInfo queueInfo)
        {
            Size = queueInfo.QueueByteSize;
            ItemsTotal = queueInfo.ItemCount;
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
    }
}