using System;

namespace Utilities.IO.Copying
{
    public class TransferRateInfo
    {
        public TransferRateInfo()
        {
            BytesTransferred = 0;
            TransferTime = TimeSpan.Zero;
        }

        public TransferRateInfo(long bytesTransferred, TimeSpan transferTime)
        {
            BytesTransferred = bytesTransferred;
            TransferTime = transferTime;
        }

        public long BytesTransferred { get; set; }
        public TimeSpan TransferTime { get; set; }
    }
}