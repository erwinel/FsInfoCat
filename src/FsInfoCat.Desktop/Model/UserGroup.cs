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
    
    public partial class UserGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserGroup()
        {
            this.IsInactive = false;
            this.Memberships = new HashSet<GroupMember>();
            this.WindowsGroupIdentities = new HashSet<WindowsGroupIdentity>();
            this.AutoAddDomains = new HashSet<WindowsAuthDomain>();
        }
    
        public System.Guid Id { get; set; }
        public string DisplayName { get; set; }
        public bool IsInactive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Guid CreatedById { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public System.Guid ModifiedById { get; set; }
        public UserRole Roles { get; set; }
        public string Notes { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroupMember> Memberships { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WindowsGroupIdentity> WindowsGroupIdentities { get; set; }
        public virtual UserAccount CreatedBy { get; set; }
        public virtual UserAccount ModifiedBy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WindowsAuthDomain> AutoAddDomains { get; set; }
    }
}
