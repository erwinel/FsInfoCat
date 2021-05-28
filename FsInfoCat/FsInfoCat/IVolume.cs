using System;

namespace FsInfoCat
{
    public interface IVolume : IDbEntity
    {
        Guid Id { get; set; }

        string DisplayName { get; set; }

        string VolumeName { get; set; }

        VolumeIdentifier Identifier { get; set; }

        bool? CaseSensitiveSearch { get; set; }

        bool? ReadOnly { get; set; }

        int? MaxNameLength { get; set; }

        System.IO.DriveType Type { get; set; }

        string Notes { get; set; }

        VolumeStatus Status { get; set; }

        IFileSystem FileSystem { get; set; }

        ISubdirectory RootDirectory { get; }
    }
}
