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
    
    public partial class DirectoryRelocateTask
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DirectoryRelocateTask()
        {
            this.Notes = "";
            this.SourceDirectories = new HashSet<Directory>();
        }
    
        public System.Guid Id { get; set; }
        public FsInfoCat.Desktop.Model.AppTaskStatus Status { get; set; }
        public FsInfoCat.Desktop.Model.PriorityLevel Priority { get; set; }
        public string ShortDescription { get; set; }
        public Nullable<System.Guid> AssignmentGroupId { get; set; }
        public Nullable<System.Guid> AssignedToId { get; set; }
        public string Notes { get; set; }
        public bool IsInactive { get; set; }
        public System.Guid TargetDirectoryId { get; set; }
        public System.Guid CreatedById { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Guid ModifiedById { get; set; }
        public System.DateTime ModifiedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Directory> SourceDirectories { get; set; }
        public virtual UserGroup AssignmentGroup { get; set; }
        public virtual UserProfile AssignedTo { get; set; }
        public virtual Directory TargetDirectory { get; set; }
        public virtual UserProfile CreatedBy { get; set; }
        public virtual UserProfile ModifiedBy { get; set; }
    }
}