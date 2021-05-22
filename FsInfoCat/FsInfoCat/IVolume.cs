namespace FsInfoCat
{
    public interface IVolume : IDbEntity
    {
        string DisplayName { get; set; }

        string VolumeName { get; set; }

        string Identifier { get; set; }

        bool? CaseSensitiveSearch { get; set; }

        bool? ReadOnly { get; set; }

        long? MaxNameLength { get; set; }

        System.IO.DriveType Type { get; set; }

        string Notes { get; set; }

        VolumeStatus Status { get; set; }

        IFileSystem FileSystem { get; set; }
    }
}
