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
    
    public partial class File
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public File()
        {
            this.Comparisons1 = new HashSet<Comparison>();
            this.Comparisons2 = new HashSet<Comparison>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public Nullable<System.Guid> CalculationId { get; set; }
        public System.Guid DirectoryId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Guid CreatedById { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public System.Guid ModifiedById { get; set; }
    
        public virtual Subdirectory ParentDirectory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comparison> Comparisons1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comparison> Comparisons2 { get; set; }
        public virtual UserAccount CreatedBy { get; set; }
        public virtual UserAccount ModifiedBy { get; set; }
        public virtual ChecksumCalculation ChecksumCalculation { get; set; }
    }
}
