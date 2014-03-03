namespace Utilities.IO.Sync
{
    public enum SyncOperationType
    {
        CopyToSource = 0,
        CopyToTarget = 1,
        DeleteSource = 2,
        DeleteTarget = 3,
        DoNothing = 4
    }

    public interface ISyncOptions
    {
        /// <summary>
        ///   Gets the operation to perform when a source file/directory exists but corresponding file/directory in target does not exist.
        /// </summary>
        SyncOperationType SourceExistsNoTarget { get; set; }

        /// <summary>
        ///   Gets the operation to perform when a target file/directory exists but corresponding file/directory in source does not exist.
        /// </summary>
        SyncOperationType TargetExistsNoSource { get; set; }

        /// <summary>
        ///   Gets the operation to perform when both source and target file/directory exists but source file/directory is newer.
        /// </summary>
        SyncOperationType SourceNewer { get; set; }

        /// <summary>
        ///   Gets the operation to perform when both source and target file/directory exists but target file/directory is newer.
        /// </summary>
        SyncOperationType TargetNewer { get; set; }

        /// <summary>
        ///   Gets the operation to perform when both source and target file/directory exists but source file/directory is larger.
        /// </summary>
        SyncOperationType SourceLarger { get; set; }

        /// <summary>
        ///   Gets the operation to perform when both source and target file/directory exists but target file/directory is larger.
        /// </summary>
        SyncOperationType TargetLarger { get; set; }
    }
}