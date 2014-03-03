using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Utilities.IO
{
    public class DriveInfo : IDriveInfo
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool GetDiskFreeSpace(string lpRootPathName,
           out uint lpSectorsPerCluster,
           out uint lpBytesPerSector,
           out uint lpNumberOfFreeClusters,
           out uint lpTotalNumberOfClusters);


        #region Fields
        private readonly string _path;
        private readonly uint _sectorsPerCluster;
        private readonly uint _bytesPerSector;
        private readonly uint _numberOfFreeClusters;
        private readonly uint _totalNumberOfClusters;
        #endregion


        #region Properties
        /// <summary>
        /// Gets path for whcih to discover information.
        /// </summary>
        public string Path
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets number of sectors per cluster.
        /// </summary>
        public int SectorsPerCluster
        {
            get { return (int)_sectorsPerCluster; }
        }

        /// <summary>
        /// Gets number of bytes per sector.
        /// </summary>
        public int BytesPerSector
        {
            get { return (int)_bytesPerSector; }
        }

        /// <summary>
        /// Gets number of free clusters.
        /// </summary>
        public long NumberOfFreeClusters
        {
            get { return _numberOfFreeClusters; }
        }

        /// <summary>
        /// Gets total number of clusters.
        /// </summary>
        public long TotalNumberOfClusters
        {
            get { return _totalNumberOfClusters; }
        }

        /// <summary>
        /// Gets available free space.
        /// </summary>
        public long AvailableFreeSpace
        {
            get
            {
                return (long)_numberOfFreeClusters * _sectorsPerCluster * _bytesPerSector;
            }
        }

        /// <summary>
        /// Gets total drive size.
        /// </summary>
        public long TotalSize
        {
            get
            {
                return (long)_totalNumberOfClusters * _sectorsPerCluster * _bytesPerSector;
            }
        }
        #endregion


        #region Constructor
        /// <summary>
        /// Creates a new <code>DriveInfo</code> object that presents information about a drive.
        /// </summary>
        /// <param name="path">Path (directory or file) on a drive. This can be a network path too.</param>
        /// <exception cref="DirectoryNotFoundException">When path does not exists.</exception>
        /// <exception cref="ArgumentNullException">When path is null.</exception>
        public DriveInfo(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path", "Path cannot be null.");
            }
            else if (!Directory.Exists(path) && !File.Exists(path))
            {
                throw new DirectoryNotFoundException(string.Format("Path {0} does not exist", path));
            }
            else if (File.Exists(path))
            {
                path = System.IO.Path.GetDirectoryName(path);
            }
            _path = path;

            GetDiskFreeSpace(_path, out _sectorsPerCluster, out _bytesPerSector,
               out _numberOfFreeClusters, out _totalNumberOfClusters);
        }
        #endregion
    }
}
