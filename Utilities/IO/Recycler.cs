using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Utilities.IO
{
    public class Recycler
    {
        private const int FO_DELETE = 3;
        private const int FOF_ALLOWUNDO = 0x40;
        private const int FOF_NOCONFIRMATION = 0x10; //No prompt dialogs 
        private const int FOF_NOERRORUI = 0x0400;
        private const int FOF_SILENT = 0x0004;
        private static SHFILEOPSTRUCT shf;

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern int SHFileOperation(ref SHFILEOPSTRUCT FileOp);
        
        private static void Recycle(string path)
        {
            shf.wFunc = FO_DELETE;
            shf.fFlags = FOF_ALLOWUNDO | FOF_NOCONFIRMATION | FOF_SILENT | FOF_NOERRORUI;
            shf.pFrom = path + '\0';
            SHFileOperation(ref shf);
        }

        /// <summary>
        /// Moves specified path (directory or file) to recycle bin.
        /// </summary>
        /// <param name="path">Path to move to recycle bin.</param>
        public static void MoveToRecycleBin(string path)
        {
            Recycle(path);
        }

        /// <summary>
        /// Permanently deletes a file or directory.
        /// </summary>
        /// <param name="fsi">File or directory to delete.</param>
        public static void Delete(FileSystemInfo fsi)
        {
            Delete(new FileSystemInfoWrapper(fsi));
        }

        /// <summary>
        /// Permanently deletes a file or directory.
        /// </summary>
        /// <param name="fsi">File or directory to delete.</param>
        public static void Delete(IFileSystemInfo fsi)
        {
            fsi.Attributes = FileAttributes.Normal;
            var di = fsi as IDirectoryInfo;
            if (di != null)
            {
                foreach (var dirInfo in di.GetFileSystemInfos())
                {
                    Delete(dirInfo);
                }
            }
            fsi.Delete();
        }


        #region Nested type: SHFILEOPSTRUCT
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
        public struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.U4)]
            public int wFunc;
            public string pFrom;
            public string pTo;
            public short fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }
        #endregion
    }
}