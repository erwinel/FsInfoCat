//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FsInfoCat.Desktop
{
    using System;
    using System.Collections.Generic;
    
    public partial class Volume
    {
        public System.Guid VolumeID { get; set; }
        public Nullable<System.Guid> HostDeviceID { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public System.Guid ModifiedBy { get; set; }
        public string DisplayName { get; set; }
        public string RootPathName { get; set; }
        public string DriveFormat { get; set; }
        public string VolumeName { get; set; }
        public string Identifier { get; set; }
        public long MaxNameLength { get; set; }
        public bool CaseSensitive { get; set; }
        public bool IsInactive { get; set; }
        public string Notes { get; set; }
    
        public virtual Account CreatedBy_Account { get; set; }
        public virtual Account ModifiedBy_Account { get; set; }
        public virtual HostDevice HostDevice { get; set; }
    }
}
