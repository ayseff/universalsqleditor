using System;
using System.IO;
using System.Security.AccessControl;

namespace Utilities.IO
{
    public class FileInfoWrapper : FileSystemInfoWrapper, IFileInfo
    {
        private readonly FileInfo _fi;

        public FileInfoWrapper(FileSystemInfo fi)
            : this(new FileInfo(fi.FullName))
        {
        }

        public FileInfoWrapper(FileInfo fi)
            : base(fi)
        {
            _fi = fi;
        }

        public FileInfoWrapper(string path)
            : this(new FileInfo(path))
        {
        }

        #region IFileInfo Members

        /// <summary>
        ///   Gets a <see cref="T:System.Security.AccessControl.FileSecurity" /> object that encapsulates the access control list (ACL) entries for the file described by the current <see
        ///    cref="T:Utilities.IO.FileInfoWrapper" /> object.
        /// </summary>
        /// <returns> A <see cref="T:System.Security.AccessControl.FileSecurity" /> object that encapsulates the access control rules for the current file. </returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows 2000 or later.</exception>
        /// <exception cref="T:System.Security.AccessControl.PrivilegeNotHeldException">The current system account does not have administrative privileges.</exception>
        /// <exception cref="T:System.SystemException">The file could not be found.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">This operation is not supported on the current platform.-or- The caller does not have the required permission.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public FileSecurity GetAccessControl()
        {
            return _fi.GetAccessControl();
        }

        /// <summary>
        ///   Gets a <see cref="T:System.Security.AccessControl.FileSecurity" /> object that encapsulates the specified type of access control list (ACL) entries for the file described by the current <see
        ///    cref="T:Utilities.IO.FileInfoWrapper" /> object.
        /// </summary>
        /// <returns> A <see cref="T:System.Security.AccessControl.FileSecurity" /> object that encapsulates the access control rules for the current file. </returns>
        /// <param name="includeSections"> One of the <see cref="T:System.Security.AccessControl.AccessControlSections" /> values that specifies which group of access control entries to retrieve. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows 2000 or later.</exception>
        /// <exception cref="T:System.Security.AccessControl.PrivilegeNotHeldException">The current system account does not have administrative privileges.</exception>
        /// <exception cref="T:System.SystemException">The file could not be found.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">This operation is not supported on the current platform.-or- The caller does not have the required permission.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public FileSecurity GetAccessControl(AccessControlSections includeSections)
        {
            return _fi.GetAccessControl(includeSections);
        }

        /// <summary>
        ///   Applies access control list (ACL) entries described by a <see cref="T:System.Security.AccessControl.FileSecurity" /> object to the file described by the current <see
        ///    cref="T:Utilities.IO.FileInfoWrapper" /> object.
        /// </summary>
        /// <param name="fileSecurity"> A <see cref="T:System.Security.AccessControl.FileSecurity" /> object that describes an access control list (ACL) entry to apply to the current file. </param>
        /// <exception cref="T:System.ArgumentNullException">The
        ///   <paramref name="fileSecurity" />
        ///   parameter is null.</exception>
        /// <exception cref="T:System.SystemException">The file could not be found or modified.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The current process does not have access to open the file.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows 2000 or later.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public void SetAccessControl(FileSecurity fileSecurity)
        {
            _fi.SetAccessControl(fileSecurity);
        }

        /// <summary>
        ///   Creates a <see cref="T:System.IO.StreamReader" /> with UTF8 encoding that reads from an existing text file.
        /// </summary>
        /// <returns> A new StreamReader with UTF8 encoding. </returns>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file is not found.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">
        ///   <paramref name="path" />
        ///   is read-only or is a directory.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public StreamReader OpenText()
        {
            return _fi.OpenText();
        }

        /// <summary>
        ///   Creates a <see cref="T:System.IO.StreamWriter" /> that writes a new text file.
        /// </summary>
        /// <returns> A new StreamWriter. </returns>
        /// <exception cref="T:System.UnauthorizedAccessException">The file name is a directory.</exception>
        /// <exception cref="T:System.IO.IOException">The disk is read-only.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public StreamWriter CreateText()
        {
            return _fi.CreateText();
        }

        /// <summary>
        ///   Creates a <see cref="T:System.IO.StreamWriter" /> that appends text to the file represented by this instance of the <see
        ///    cref="T:Utilities.IO.FileInfoWrapper" /> .
        /// </summary>
        /// <returns> A new StreamWriter. </returns>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public StreamWriter AppendText()
        {
            return _fi.AppendText();
        }

        /// <summary>
        ///   Copies an existing file to a new file, disallowing the overwriting of an existing file.
        /// </summary>
        /// <returns> A new file with a fully qualified path. </returns>
        /// <param name="destFileName"> The name of the new file to copy to. </param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="destFileName" />
        ///   is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="T:System.IO.IOException">An error occurs, or the destination file already exists.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="destFileName" />
        ///   is null.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">A directory path is passed in, or the file is being moved to a different drive.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The directory specified in
        ///   <paramref name="destFileName" />
        ///   does not exist.</exception>
        /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="T:System.NotSupportedException">
        ///   <paramref name="destFileName" />
        ///   contains a colon (:) within the string but does not specify the volume.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public IFileInfo CopyTo(string destFileName)
        {
            return new FileInfoWrapper(_fi.CopyTo(destFileName));
        }

        /// <summary>
        ///   Copies an existing file to a new file, allowing the overwriting of an existing file.
        /// </summary>
        /// <returns> A new file, or an overwrite of an existing file if <paramref name="overwrite" /> is true. If the file exists and <paramref
        ///    name="overwrite" /> is false, an <see cref="T:System.IO.IOException" /> is thrown. </returns>
        /// <param name="destFileName"> The name of the new file to copy to. </param>
        /// <param name="overwrite"> true to allow an existing file to be overwritten; otherwise, false. </param>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="destFileName" />
        ///   is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="T:System.IO.IOException">An error occurs, or the destination file already exists and
        ///   <paramref name="overwrite" />
        ///   is false.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="destFileName" />
        ///   is null.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The directory specified in
        ///   <paramref name="destFileName" />
        ///   does not exist.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">A directory path is passed in, or the file is being moved to a different drive.</exception>
        /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="T:System.NotSupportedException">
        ///   <paramref name="destFileName" />
        ///   contains a colon (:) in the middle of the string.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public IFileInfo CopyTo(string destFileName, bool overwrite)
        {
            return new FileInfoWrapper(_fi.CopyTo(destFileName, overwrite));
        }

        /// <summary>
        ///   Creates a file.
        /// </summary>
        /// <returns> A new file. </returns>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public FileStream Create()
        {
            return _fi.Create();
        }

        ///// <summary>
        /////   Permanently deletes a file.
        ///// </summary>
        ///// <exception cref="T:System.IO.IOException">The target file is open or memory-mapped on a computer running Microsoft Windows NT.</exception>
        ///// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        ///// <exception cref="T:System.UnauthorizedAccessException">The path is a directory.</exception>
        ///// <filterpriority>1</filterpriority>
        ///// <PermissionSet>
        /////   <IPermission
        /////     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        /////     version="1" Unrestricted="true" />
        ///// </PermissionSet>
        //public void Delete()
        //{
        //    _fi.Delete();
        //}

        /// <summary>
        ///   Decrypts a file that was encrypted by the current account using the <see cref="M:Utilities.IO.FileInfoWrapper.Encrypt" /> method.
        /// </summary>
        /// <exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file described by the current
        ///   <see cref="T:Utilities.IO.FileInfoWrapper" />
        ///   object could not be found.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="T:System.NotSupportedException">The file system is not NTFS.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The file described by the current
        ///   <see cref="T:Utilities.IO.FileInfoWrapper" />
        ///   object is read-only.-or- This operation is not supported on the current platform.-or- The caller does not have the required permission.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public void Decrypt()
        {
            _fi.Decrypt();
        }

        /// <summary>
        ///   Encrypts a file so that only the account used to encrypt the file can decrypt it.
        /// </summary>
        /// <exception cref="T:System.IO.DriveNotFoundException">An invalid drive was specified.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file described by the current
        ///   <see cref="T:Utilities.IO.FileInfoWrapper" />
        ///   object could not be found.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="T:System.NotSupportedException">The file system is not NTFS.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The file described by the current
        ///   <see cref="T:Utilities.IO.FileInfoWrapper" />
        ///   object is read-only.-or- This operation is not supported on the current platform.-or- The caller does not have the required permission.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public void Encrypt()
        {
            _fi.Encrypt();
        }

        /// <summary>
        ///   Opens a file in the specified mode.
        /// </summary>
        /// <returns> A file opened in the specified mode, with read/write access and unshared. </returns>
        /// <param name="mode"> A <see cref="T:System.IO.FileMode" /> constant specifying the mode (for example, Open or Append) in which to open the file. </param>
        /// <exception cref="T:System.IO.FileNotFoundException">The file is not found.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The file is read-only or is a directory.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.IO.IOException">The file is already open.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public FileStream Open(FileMode mode)
        {
            return _fi.Open(mode);
        }

        /// <summary>
        ///   Opens a file in the specified mode with read, write, or read/write access.
        /// </summary>
        /// <returns> A <see cref="T:System.IO.FileStream" /> object opened in the specified mode and access, and unshared. </returns>
        /// <param name="mode"> A <see cref="T:System.IO.FileMode" /> constant specifying the mode (for example, Open or Append) in which to open the file. </param>
        /// <param name="access"> A <see cref="T:System.IO.FileAccess" /> constant specifying whether to open the file with Read, Write, or ReadWrite file access. </param>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="path" />
        ///   is empty or contains only white spaces.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file is not found.</exception>
        /// <exception cref="T:System.ArgumentNullException">One or more arguments is null.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">
        ///   <paramref name="path" />
        ///   is read-only or is a directory.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.IO.IOException">The file is already open.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public FileStream Open(FileMode mode, FileAccess access)
        {
            return _fi.Open(mode, access);
        }

        /// <summary>
        ///   Opens a file in the specified mode with read, write, or read/write access and the specified sharing option.
        /// </summary>
        /// <returns> A <see cref="T:System.IO.FileStream" /> object opened with the specified mode, access, and sharing options. </returns>
        /// <param name="mode"> A <see cref="T:System.IO.FileMode" /> constant specifying the mode (for example, Open or Append) in which to open the file. </param>
        /// <param name="access"> A <see cref="T:System.IO.FileAccess" /> constant specifying whether to open the file with Read, Write, or ReadWrite file access. </param>
        /// <param name="share"> A <see cref="T:System.IO.FileShare" /> constant specifying the type of access other FileStream objects have to this file. </param>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="path" />
        ///   is empty or contains only white spaces.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file is not found.</exception>
        /// <exception cref="T:System.ArgumentNullException">One or more arguments is null.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">
        ///   <paramref name="path" />
        ///   is read-only or is a directory.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.IO.IOException">The file is already open.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public FileStream Open(FileMode mode, FileAccess access, FileShare share)
        {
            return _fi.Open(mode, access, share);
        }

        /// <summary>
        ///   Creates a read-only <see cref="T:System.IO.FileStream" /> .
        /// </summary>
        /// <returns> A new read-only <see cref="T:System.IO.FileStream" /> object. </returns>
        /// <exception cref="T:System.UnauthorizedAccessException">
        ///   <paramref name="path" />
        ///   is read-only or is a directory.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.IO.IOException">The file is already open.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public FileStream OpenRead()
        {
            return _fi.OpenRead();
        }

        /// <summary>
        ///   Creates a write-only <see cref="T:System.IO.FileStream" /> .
        /// </summary>
        /// <returns> A new write-only unshared <see cref="T:System.IO.FileStream" /> object. </returns>
        /// <exception cref="T:System.UnauthorizedAccessException">
        ///   <paramref name="path" />
        ///   is read-only or is a directory.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public FileStream OpenWrite()
        {
            return _fi.OpenWrite();
        }

        /// <summary>
        ///   Moves a specified file to a new location, providing the option to specify a new file name.
        /// </summary>
        /// <param name="destFileName"> The path to move the file to, which can specify a different file name. </param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs, such as the destination file already exists or the destination device is not ready.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="destFileName" />
        ///   is null.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="destFileName" />
        ///   is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">
        ///   <paramref name="destFileName" />
        ///   is read-only or is a directory.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file is not found.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.IO.PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="T:System.NotSupportedException">
        ///   <paramref name="destFileName" />
        ///   contains a colon (:) in the middle of the string.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public void MoveTo(string destFileName)
        {
            _fi.MoveTo(destFileName);
        }

        /// <summary>
        ///   Replaces the contents of a specified file with the file described by the current <see
        ///    cref="T:Utilities.IO.FileInfoWrapper" /> object, deleting the original file, and creating a backup of the replaced file.
        /// </summary>
        /// <returns> A <see cref="T:Utilities.IO.FileInfoWrapper" /> object that encapsulates information about the file described by the <paramref
        ///    name="destFileName" /> parameter. </returns>
        /// <param name="destFileName"> The name of a file to replace with the current file. </param>
        /// <param name="destBackupFileName"> The name of a file with which to create a backup of the file described by the <paramref
        ///    name="destFileName" /> parameter. </param>
        /// <exception cref="T:System.ArgumentException">The path described by the
        ///   <paramref name="destFileName" />
        ///   parameter was not of a legal form.-or-The path described by the
        ///   <paramref name="destBackupFileName" />
        ///   parameter was not of a legal form.</exception>
        /// <exception cref="T:System.ArgumentNullException">The
        ///   <paramref name="destFileName" />
        ///   parameter is null.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file described by the current
        ///   <see cref="T:Utilities.IO.FileInfoWrapper" />
        ///   object could not be found.-or-The file described by the
        ///   <paramref name="destFileName" />
        ///   parameter could not be found.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public IFileInfo Replace(string destFileName, string destBackupFileName)
        {
            return new FileInfoWrapper(_fi.Replace(destFileName, destBackupFileName));
        }

        /// <summary>
        ///   Replaces the contents of a specified file with the file described by the current <see
        ///    cref="T:Utilities.IO.FileInfoWrapper" /> object, deleting the original file, and creating a backup of the replaced file. Also specifies whether to ignore merge errors.
        /// </summary>
        /// <returns> A <see cref="T:Utilities.IO.FileInfoWrapper" /> object that encapsulates information about the file described by the <paramref
        ///    name="destFileName" /> parameter. </returns>
        /// <param name="destFileName"> The name of a file to replace with the current file. </param>
        /// <param name="destBackupFileName"> The name of a file with which to create a backup of the file described by the <paramref
        ///    name="destFileName" /> parameter. </param>
        /// <param name="ignoreMetadataErrors"> true to ignore merge errors (such as attributes and ACLs) from the replaced file to the replacement file; otherwise false. </param>
        /// <exception cref="T:System.ArgumentException">The path described by the
        ///   <paramref name="destFileName" />
        ///   parameter was not of a legal form.-or-The path described by the
        ///   <paramref name="destBackupFileName" />
        ///   parameter was not of a legal form.</exception>
        /// <exception cref="T:System.ArgumentNullException">The
        ///   <paramref name="destFileName" />
        ///   parameter is null.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file described by the current
        ///   <see cref="T:Utilities.IO.FileInfoWrapper" />
        ///   object could not be found.-or-The file described by the
        ///   <paramref name="destFileName" />
        ///   parameter could not be found.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public IFileInfo Replace(string destFileName, string destBackupFileName, bool ignoreMetadataErrors)
        {
            return new FileInfoWrapper(_fi.Replace(destFileName, destBackupFileName, ignoreMetadataErrors));
        }

        ///// <summary>
        /////   Gets the name of the file.
        ///// </summary>
        ///// <returns> The name of the file. </returns>
        ///// <filterpriority>1</filterpriority>
        //public string Name
        //{
        //    get { return _fi.Name; }
        //}

        /// <summary>
        ///   Gets the size, in bytes, of the current file.
        /// </summary>
        /// <returns> The size of the current file in bytes. </returns>
        /// <exception cref="T:System.IO.IOException">
        ///   <see cref="M:Utilities.IO.FileSystemInfoWrapper.Refresh" />
        ///   cannot update the state of the file or directory.</exception>
        /// <exception cref="T:System.IO.FileNotFoundException">The file does not exist.-or- The Length property is called for a directory.</exception>
        /// <filterpriority>1</filterpriority>
        public long Length
        {
            get { return _fi.Length; }
        }

        /// <summary>
        ///   Gets or sets a value that determines if the current file is read only.
        /// </summary>
        /// <returns> true if the current file is read only; otherwise, false. </returns>
        /// <exception cref="T:System.IO.FileNotFoundException">The file described by the current
        ///   <see cref="T:Utilities.IO.FileInfoWrapper" />
        ///   object could not be found.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="T:System.UnauthorizedAccessException">The file described by the current
        ///   <see cref="T:Utilities.IO.FileInfoWrapper" />
        ///   object is read-only.-or- This operation is not supported on the current platform.-or- The caller does not have the required permission.</exception>
        /// <filterpriority>1</filterpriority>
        public bool IsReadOnly
        {
            get { return _fi.IsReadOnly; }
            set { _fi.IsReadOnly = value; }
        }

        /// <summary>
        ///   Gets an instance of the parent directory.
        /// </summary>
        /// <returns> A <see cref="T:Utilities.IO.DirectoryInfoWrapper" /> object representing the parent directory of this file. </returns>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public IDirectoryInfo Directory
        {
            get { return new DirectoryInfoWrapper(_fi.Directory); }
        }

        /// <summary>
        ///   Gets a string representing the directory's full path.
        /// </summary>
        /// <returns> A string representing the directory's full path. </returns>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.ArgumentNullException">null was passed in for the directory name.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public string DirectoryName
        {
            get { return _fi.DirectoryName; }
        }

        /// <summary>
        ///  Compares file to another on name, size, last write time, attributes and hash of the contents.
        /// </summary>
        /// <param name="file">Comparison file.</param>
        /// <returns> <code>true</code> if files are identical, <code>false</code> otherise. </returns>
        /// <remarks>
        /// This method does not compare actual file contents but rather it compares the hash values of the contents using SHA1 algorithm.
        /// </remarks>
        public override bool IsIdenticalTo(IFileSystemInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file", "File is null.");
            }

            IFileInfo fileInfo = file as IFileInfo;
            if (fileInfo == null)
            {
                return false;
            }

            if (!base.IsIdenticalTo(file)
                || Length != fileInfo.Length
                || this.ComputeSHA1() != fileInfo.ComputeSHA1())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///  Compares <code>IFileSystemInfo</code> to <code>FileSystemInfo</code> on name, last write time and attributes.
        /// </summary>
        /// <param name="fileSystemInfo">Comparison <code>FileSystemInfo</code>.</param>
        /// <returns> <code>true</code> if <code>IFileSystemInfo</code> and <code>FileSystemInfo</code> are identical, <code>false</code> otherwise. </returns>
        /// <remarks>
        /// This method does not compare actual file contents but rather it compares the hash values of the contents using SHA1 algorithm.
        /// </remarks>
        public override bool IsIdenticalTo(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo == null)
            {
                throw new ArgumentNullException("fileSystemInfo", "File is null.");
            }

            FileInfo fileInfo = fileSystemInfo as FileInfo;
            if (fileInfo == null)
            {
                return false;
            }

            if (!base.IsIdenticalTo(fileSystemInfo)
                || Length != fileInfo.Length
                || IsReadOnly != fileInfo.IsReadOnly
                || this.ComputeSHA1() != fileInfo.ComputeSHA1())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///   Gets size.
        /// </summary>
        public override long Size
        {
            get { return _fi.Length; }
        }
        #endregion
    }
}