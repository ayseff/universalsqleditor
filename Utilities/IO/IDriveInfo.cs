namespace Utilities.IO
{
    public interface IDriveInfo
    {
        /// <summary>
        /// Gets path for whcih to discover information.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Gets number of sectors per cluster.
        /// </summary>
        int SectorsPerCluster { get; }

        /// <summary>
        /// Gets number of bytes per sector.
        /// </summary>
        int BytesPerSector { get; }

        /// <summary>
        /// Gets number of free clusters.
        /// </summary>
        long NumberOfFreeClusters { get; }

        /// <summary>
        /// Gets total number of clusters.
        /// </summary>
        long TotalNumberOfClusters { get; }

        /// <summary>
        /// Gets available free space.
        /// </summary>
        long AvailableFreeSpace { get; }

        /// <summary>
        /// Gets total drive size.
        /// </summary>
        long TotalSize { get; }
    }
}