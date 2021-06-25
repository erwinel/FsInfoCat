using System.ComponentModel;

namespace FsInfoCat
{
    public enum AccessErrorCode : byte
    {
        Unspecified = 0,

        /// <summary>
        /// An <see cref="System.IO.IOException"/> has occurred.
        /// </summary>
        [AmbientValue(ErrorCode.IOError)]
        IOError = ErrorCode.IOError,

        /// <summary>
        /// An <see cref="System.UnauthorizedAccessException"/> has occurred.
        /// </summary>
        [AmbientValue(ErrorCode.UnauthorizedAccess)]
        UnauthorizedAccess = ErrorCode.UnauthorizedAccess,

        /// <summary>
        /// A <see cref="System.Security.SecurityException"/> has occurred.
        /// </summary>
        [AmbientValue(ErrorCode.SecurityException)]
        SecurityException = ErrorCode.SecurityException,

        /// <summary>
        /// The name of one or more files or subdirectories in the path contain a invalid character(s).
        /// </summary>
        [AmbientValue(ErrorCode.InvalidPath)]
        InvalidPath = ErrorCode.InvalidPath,

        /// <summary>
        /// A <see cref="System.IO.PathTooLongException"/> has occurred, indicating that hhe specified path, file name, or both exceed the system-defined maximum length.
        /// </summary>
        [AmbientValue(ErrorCode.PathTooLong)]
        PathTooLong = ErrorCode.PathTooLong
    }
}