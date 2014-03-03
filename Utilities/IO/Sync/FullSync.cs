using System;

namespace Utilities.IO.Sync
{
    public class FullSync : ISyncOptions
    {
        #region ISyncOptions Members

        /// <summary>
        ///   Gets the operation to perform when a source file/directory exists but corresponding file/directory in target does not exist.
        /// </summary>
        public SyncOperationType SourceExistsNoTarget
        {
            get { return SyncOperationType.CopyToTarget; }
            set { throw new InvalidOperationException(); }
        }

        /// <summary>
        ///   Gets the operation to perform when a target file/directory exists but corresponding file/directory in source does not exist.
        /// </summary>
        public SyncOperationType TargetExistsNoSource
        {
            get { return SyncOperationType.CopyToSource; }
            set { throw new InvalidOperationException(); }
        }

        /// <summary>
        ///   Gets the operation to perform when both source and target file/directory exists but source file/directory is newer.
        /// </summary>
        public SyncOperationType SourceNewer
        {
            get { return SyncOperationType.CopyToTarget; }
            set { throw new InvalidOperationException(); }
        }

        /// <summary>
        ///   Gets the operation to perform when both source and target file/directory exists but target file/directory is newer.
        /// </summary>
        public SyncOperationType TargetNewer
        {
            get { return SyncOperationType.CopyToSource; }
            set { throw new InvalidOperationException(); }
        }

        /// <summary>
        ///   Gets the operation to perform when both source and target file/directory exists but source file/directory is larger.
        /// </summary>
        public SyncOperationType SourceLarger
        {
            get { return SyncOperationType.CopyToTarget; }
            set { throw new InvalidOperationException(); }
        }

        /// <summary>
        ///   Gets the operation to perform when both source and target file/directory exists but target file/directory is larger.
        /// </summary>
        public SyncOperationType TargetLarger
        {
            get { return SyncOperationType.CopyToSource; }
            set { throw new InvalidOperationException(); }
        }

        #endregion
    }
}