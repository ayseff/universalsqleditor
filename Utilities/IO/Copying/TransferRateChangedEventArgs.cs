using System;

namespace Utilities.IO.Copying
{
    [Serializable]
    public class TransferRateChangedEventArgs : System.EventArgs
    {
        private readonly TransferRateInfo _transferRateInfo;

        /// <summary>
        ///   Initializes TransferRateChangedEventArgs object.
        /// </summary>
        /// <param name="transferRateInfo">Transfer rate information.</param>
        public TransferRateChangedEventArgs(TransferRateInfo transferRateInfo)
        {
            _transferRateInfo = transferRateInfo;
        }

        public TransferRateInfo TransferRateInfo
        {
            get { return _transferRateInfo; }
        }
    }
}