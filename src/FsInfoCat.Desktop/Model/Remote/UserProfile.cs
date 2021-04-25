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
    
    public partial class UserProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserProfile()
        {
            this.Notes = "\"\"";
            this.IsInactive = false;
            this.CreatedSymbolicNames = new HashSet<FsSymbolicName>();
            this.CreatedComparisons = new HashSet<Comparison>();
            this.CreatedDirectories = new HashSet<Directory>();
            this.CreatedFiles = new HashSet<File>();
            this.CreatedFileSystems = new HashSet<FileSystem>();
            this.CreatedHashCalculations = new HashSet<HashCalculation>();
            this.CreatedHostDevices = new HashSet<HostDevice>();
            this.CreatedHostPlatforms = new HashSet<HostPlatform>();
            this.CreatedRedundancies = new HashSet<Redundancy>();
            this.CreatedUserProfiles = new HashSet<UserProfile>();
            this.CreatedVolumes = new HashSet<Volume>();
            this.ModifiedVolumes = new HashSet<Volume>();
            this.ModifiedUserProfiles = new HashSet<UserProfile>();
            this.ModifiedRedundancies = new HashSet<Redundancy>();
            this.ModifiedHostPlatforms = new HashSet<HostPlatform>();
            this.ModifiedHostDevices = new HashSet<HostDevice>();
            this.ModifiedHashCalculations = new HashSet<HashCalculation>();
            this.ModifiedSymbolicNames = new HashSet<FsSymbolicName>();
            this.ModifiedFileSystems = new HashSet<FileSystem>();
            this.ModifiedFiles = new HashSet<File>();
            this.ModifiedDirectories = new HashSet<Directory>();
            this.ModifiedComparisons = new HashSet<Comparison>();
        }
    
        public System.Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MI { get; set; }
        public string Suffix { get; set; }
        public string Title { get; set; }
        public Nullable<int> DbPrincipalId { get; set; }
        public byte[] SID { get; set; }
        public string LoginName { get; set; }
        public UserRole ExplicitRoles { get; set; }
        public string Notes { get; set; }
        public bool IsInactive { get; set; }
        public System.Guid CreatedById { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Guid ModifiedById { get; set; }
        public System.DateTime ModifiedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FsSymbolicName> CreatedSymbolicNames { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comparison> CreatedComparisons { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Directory> CreatedDirectories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<File> CreatedFiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileSystem> CreatedFileSystems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HashCalculation> CreatedHashCalculations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HostDevice> CreatedHostDevices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HostPlatform> CreatedHostPlatforms { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Redundancy> CreatedRedundancies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserProfile> CreatedUserProfiles { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Volume> CreatedVolumes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Volume> ModifiedVolumes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserProfile> ModifiedUserProfiles { get; set; }
        public virtual UserProfile ModifiedBy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Redundancy> ModifiedRedundancies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HostPlatform> ModifiedHostPlatforms { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HostDevice> ModifiedHostDevices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HashCalculation> ModifiedHashCalculations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FsSymbolicName> ModifiedSymbolicNames { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FileSystem> ModifiedFileSystems { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<File> ModifiedFiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Directory> ModifiedDirectories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comparison> ModifiedComparisons { get; set; }
    }
}
