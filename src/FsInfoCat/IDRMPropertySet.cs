namespace FsInfoCat
{
    /// <summary>
    /// Interface for database objects that contain extended file DRM property values.
    /// </summary>
    /// <seealso cref="IDRMProperties" />
    /// <seealso cref="IPropertySet" />
    /// <seealso cref="Local.ILocalDRMPropertySet"/>
    /// <seealso cref="Upstream.IUpstreamDRMPropertySet"/>
    public interface IDRMPropertySet : IDRMProperties, IPropertySet
    {
    }
}
