using System;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Serialization;

namespace Utilities.IO
{
    public class FileSystemInfoWrapper : IFileSystemInfo
    {
        private readonly FileSystemInfo _fsi;

        public FileSystemInfoWrapper(FileSystemInfo fsi)
        {
            _fsi = fsi;
        }

        #region IFileSystemInfo Members

        /// <summary>
        ///   Retrieves the current lifetime service object that controls the lifetime policy for this instance.
        /// </summary>
        /// <returns> An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease" /> used to control the lifetime policy for this instance. </returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Flags="RemotingConfiguration, Infrastructure" />
        /// </PermissionSet>
        public object GetLifetimeService()
        {
            return _fsi.GetLifetimeService();
        }

        /// <summary>
        ///   Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns> An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease" /> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see
        ///    cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime" /> property. </returns>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Flags="RemotingConfiguration, Infrastructure" />
        /// </PermissionSet>
        public object InitializeLifetimeService()
        {
            return _fsi.InitializeLifetimeService();
        }

        /// <summary>
        ///   Creates an object that contains all the relevant information required to generate a proxy used to communicate with a remote object.
        /// </summary>
        /// <returns> Information required to generate a proxy. </returns>
        /// <param name="requestedType"> The <see cref="T:System.Type" /> of the object that the new <see
        ///    cref="T:System.Runtime.Remoting.ObjRef" /> will reference. </param>
        /// <exception cref="T:System.Runtime.Remoting.RemotingException">This instance is not a valid remoting object.</exception>
        /// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission.</exception>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Flags="Infrastructure" />
        /// </PermissionSet>
        public ObjRef CreateObjRef(Type requestedType)
        {
            return _fsi.CreateObjRef(requestedType);
        }

        /// <summary>
        ///   Deletes a file or directory.
        /// </summary>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <filterpriority>2</filterpriority>
        public void Delete()
        {
            _fsi.Delete();
        }

        /// <summary>
        ///   Refreshes the state of the object.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">A device such as a disk drive is not ready.</exception>
        /// <filterpriority>1</filterpriority>
        public void Refresh()
        {
            _fsi.Refresh();
        }

        /// <summary>
        ///   Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the file name and additional exception information.
        /// </summary>
        /// <param name="info"> The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
        /// <param name="context"> The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
        /// <filterpriority>2</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        ///   <IPermission
        ///     class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Flags="SerializationFormatter" />
        /// </PermissionSet>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            _fsi.GetObjectData(info, context);
        }

        /// <summary>
        ///  Compares <code>IFileSystemInfo</code> to another on name, last write time and attributes.
        /// </summary>
        /// <param name="fileSystemInfo">Comparison <code>IFileSystemInfo</code>.</param>
        /// <returns> <code>true</code> if <code>IFileSystemInfo</code> are identical, <code>false</code> otherise. </returns>
        public virtual bool IsIdenticalTo(IFileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo == null)
            {
                return false;
            }

            if (Name != fileSystemInfo.Name
                || LastWriteTime.Subtract(fileSystemInfo.LastWriteTime).TotalSeconds >= 1
                || (Attributes & fileSystemInfo.Attributes) != Attributes)
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
        public virtual bool IsIdenticalTo(FileSystemInfo fileSystemInfo)
        {
            if (fileSystemInfo == null)
            {
                throw new ArgumentNullException("fileSystemInfo", "File is null.");
            }

            if (Name != fileSystemInfo.Name
                || !LastWriteTime.Equals(fileSystemInfo.LastWriteTime)
                || (Attributes & fileSystemInfo.Attributes) != Attributes)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///   Gets the full path of the directory or file.
        /// </summary>
        /// <returns> A string containing the full path. </returns>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public string FullName
        {
            get { return _fsi.FullName; }
        }

        /// <summary>
        ///   Gets the string representing the extension part of the file.
        /// </summary>
        /// <returns> A string containing the <see cref="T:Utilities.IO.FileSystemInfoWrapper" /> extension. </returns>
        /// <filterpriority>1</filterpriority>
        public string Extension
        {
            get { return _fsi.Extension; }
        }

        /// <summary>
        ///   For files, gets the name of the file. For directories, gets the name of the last directory in the hierarchy if a hierarchy exists. Otherwise, the Name property gets the name of the directory.
        /// </summary>
        /// <returns> A string that is the name of the parent directory, the name of the last directory in the hierarchy, or the name of a file, including the file name extension. </returns>
        /// <filterpriority>1</filterpriority>
        public string Name
        {
            get { return _fsi.Name; }
        }

        /// <summary>
        ///   Gets a value indicating whether the file or directory exists.
        /// </summary>
        /// <returns> true if the file or directory exists; otherwise, false. </returns>
        /// <filterpriority>1</filterpriority>
        public bool Exists
        {
            get { return _fsi.Exists; }
        }

        /// <summary>
        ///   Gets or sets the creation time of the current file or directory.
        /// </summary>
        /// <returns> The creation date and time of the current <see cref="T:Utilities.IO.FileSystemInfoWrapper" /> object. </returns>
        /// <exception cref="T:System.IO.IOException">
        ///   <see cref="M:Utilities.IO.FileSystemInfoWrapper.Refresh" />
        ///   cannot initialize the data.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public DateTime CreationTime
        {
            get { return _fsi.CreationTime; }
            set { _fsi.CreationTime = value; }
        }

        /// <summary>
        ///   Gets or sets the creation time, in coordinated universal time (UTC), of the current file or directory.
        /// </summary>
        /// <returns> The creation date and time in UTC format of the current <see cref="T:Utilities.IO.FileSystemInfoWrapper" /> object. </returns>
        /// <exception cref="T:System.IO.IOException">
        ///   <see cref="M:Utilities.IO.FileSystemInfoWrapper.Refresh" />
        ///   cannot initialize the data.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public DateTime CreationTimeUtc
        {
            get { return _fsi.CreationTimeUtc; }
            set { _fsi.CreationTimeUtc = value; }
        }

        /// <summary>
        ///   Gets or sets the time the current file or directory was last accessed.
        /// </summary>
        /// <returns> The time that the current file or directory was last accessed. </returns>
        /// <exception cref="T:System.IO.IOException">
        ///   <see cref="M:Utilities.IO.FileSystemInfoWrapper.Refresh" />
        ///   cannot initialize the data.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public DateTime LastAccessTime
        {
            get { return _fsi.LastAccessTime; }
            set { _fsi.LastAccessTime = value; }
        }

        /// <summary>
        ///   Gets or sets the time, in coordinated universal time (UTC), that the current file or directory was last accessed.
        /// </summary>
        /// <returns> The UTC time that the current file or directory was last accessed. </returns>
        /// <exception cref="T:System.IO.IOException">
        ///   <see cref="M:Utilities.IO.FileSystemInfoWrapper.Refresh" />
        ///   cannot initialize the data.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public DateTime LastAccessTimeUtc
        {
            get { return _fsi.LastAccessTimeUtc; }
            set { _fsi.LastAccessTimeUtc = value; }
        }

        /// <summary>
        ///   Gets or sets the time when the current file or directory was last written to.
        /// </summary>
        /// <returns> The time the current file was last written. </returns>
        /// <exception cref="T:System.IO.IOException">
        ///   <see cref="M:Utilities.IO.FileSystemInfoWrapper.Refresh" />
        ///   cannot initialize the data.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public DateTime LastWriteTime
        {
            get { return _fsi.LastWriteTime; }
            set { _fsi.LastWriteTime = value; }
        }

        /// <summary>
        ///   Gets or sets the time, in coordinated universal time (UTC), when the current file or directory was last written to.
        /// </summary>
        /// <returns> The UTC time when the current file was last written to. </returns>
        /// <exception cref="T:System.IO.IOException">
        ///   <see cref="M:Utilities.IO.FileSystemInfoWrapper.Refresh" />
        ///   cannot initialize the data.</exception>
        /// <exception cref="T:System.PlatformNotSupportedException">The current operating system is not Microsoft Windows NT or later.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public DateTime LastWriteTimeUtc
        {
            get { return _fsi.LastWriteTimeUtc; }
            set { _fsi.LastWriteTimeUtc = value; }
        }

        /// <summary>
        ///   Gets or sets the attributes for the current file or directory.
        /// </summary>
        /// <returns> <see cref="T:System.IO.FileAttributes" /> of the current <see cref="T:Utilities.IO.FileSystemInfoWrapper" /> . </returns>
        /// <exception cref="T:System.IO.FileNotFoundException">The specified file does not exist.</exception>
        /// <exception cref="T:System.IO.DirectoryNotFoundException">The specified path is invalid, such as being on an unmapped drive.</exception>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="T:System.ArgumentException">The caller attempts to set an invalid file attribute.</exception>
        /// <exception cref="T:System.IO.IOException">
        ///   <see cref="M:Utilities.IO.FileSystemInfoWrapper.Refresh" />
        ///   cannot initialize the data.</exception>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///   <IPermission
        ///     class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///     version="1" Unrestricted="true" />
        /// </PermissionSet>
        public FileAttributes Attributes
        {
            get { return _fsi.Attributes; }
            set { _fsi.Attributes = value; }
        }

        protected long _size = -1;

        /// <summary>
        ///   Gets size.
        /// </summary>
        public virtual long Size
        {
            get { throw new NotImplementedException("Size cannot be called on FileSystemInfoWrapper object."); }
            protected set { _size = value; }
        }

        #endregion

        /// <summary>
        /// Returns ToString representation of the object.
        /// </summary>
        /// <returns>ToString representation of the object.</returns>
        public override string ToString()
        {
            return FullName;
        }
    }
}