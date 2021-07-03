namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file property values of video files.
    /// </summary>
    /// <seealso cref="IVideoProperties" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="Local.ILocalVideoPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamVideoPropertySet"/>
    /// <seealso cref="IFile.VideoProperties"/>
    public interface IVideoPropertySet : IVideoProperties, IPropertySet
    {
    }
}
