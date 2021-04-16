//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FsInfoCat.Desktop.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class HostDevice
    {
        public HostDevice()
        {
            this.Volumes = new HashSet<Volume>();
        }
    
        public System.Guid HostDeviceID { get; set; }
        public string DisplayName { get; set; }
        public string MachineIdentifer { get; set; }
        public string MachineName { get; set; }
        public byte Platform { get; set; }
        public bool AllowCrawl { get; set; }
        public bool IsInactive { get; set; }
        public string Notes { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public System.Guid ModifiedBy { get; set; }
    
        public virtual Account CreatedBy_Account { get; set; }
        public virtual Account ModifiedBy_Account { get; set; }
        public virtual ICollection<Volume> Volumes { get; set; }
    }
}