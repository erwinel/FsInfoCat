//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FsInfoCat.Desktop.Model
{
    using System;
    using System.Collections.Generic;

    public partial class Volume
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Volume()
        {
            this.CaseSensitive = false;
            this.Notes = "";
            this.Subdirectories = new HashSet<Subdirectory>();
        }

        public System.Guid Id { get; set; }
        public Nullable<System.Guid> HostDeviceId { get; set; }
        public string DisplayName { get; set; }
        public string RootPathName { get; set; }
        public string DriveFormat { get; set; }
        public string VolumeName { get; set; }
        public string Identifier { get; set; }
        public long MaxNameLength { get; set; }
        public bool CaseSensitive { get; set; }
        public bool IsInactive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Guid CreatedById { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public System.Guid ModifiedById { get; set; }
        public string Notes { get; set; }

        public virtual UserAccount CreatedBy { get; set; }
        public virtual UserAccount ModifiedBy { get; set; }
        public virtual HostDevice HostDevice { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subdirectory> Subdirectories { get; set; }
    }
}
