//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FsInfoCat.Desktop.Model.Remote
{
    using System;
    using System.Collections.Generic;
    
    public partial class FileSystem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FileSystem()
        {
            this.Notes = "\"\"";
            this.DefaultHostPlatforms = new HashSet<HostPlatform>();
            this.Volumes = new HashSet<Volume>();
            this.SymbolicNames = new HashSet<FsSymbolicName>();
        }
    
        public System.Guid Id { get; set; }
        public string DisplayName { get; set; }
        public bool CaseSensitiveSearch { get; set; }
        public bool ReadOnly { get; set; }
        public long MaxNameLength { get; set; }
        public Nullable<System.IO.DriveType> DefaultDriveType { get; set; }
        public string Notes { get; set; }
        public bool IsInactive { get; set; }
        public System.Guid CreatedById { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Guid ModifiedById { get; set; }
        public System.DateTime ModifiedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HostPlatform> DefaultHostPlatforms { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Volume> Volumes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FsSymbolicName> SymbolicNames { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public virtual UserProfile ModifiedBy { get; set; }
    }
}
