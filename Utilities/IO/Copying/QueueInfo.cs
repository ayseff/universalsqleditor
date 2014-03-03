namespace Utilities.IO.Copying
{
    public class QueueInfo
    {
        public QueueInfo()
        {
            ItemCount = 0;
            ItemsCompleteCount = 0;
            QueueByteSize = 0;
            QueueBytesTransferred = 0;
        }

        public QueueInfo(int itemCount)
            : this()
        {
            ItemCount = itemCount;
        }

        public int ItemCount { get; set; }
        public int ItemsCompleteCount { get; set; }
        public long QueueByteSize { get; set; }
        public long QueueBytesTransferred { get; set; }
    }
}