namespace Utilities.IO.Sync
{
    public class CustomSync : ISyncOptions
    {
        #region ISyncOptions Members

        public SyncOperationType SourceExistsNoTarget { get; set; }

        public SyncOperationType TargetExistsNoSource { get; set; }

        public SyncOperationType SourceNewer { get; set; }

        public SyncOperationType TargetNewer { get; set; }

        public SyncOperationType SourceLarger { get; set; }

        public SyncOperationType TargetLarger { get; set; }

        #endregion
    }
}