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
    
    public partial class UserCredential
    {
        public System.Guid AccountID { get; set; }
        public string PwHash { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.Guid CreatedBy { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public System.Guid ModifiedBy { get; set; }
    
        public virtual Account CreatedBy_Account { get; set; }
        public virtual Account ModifiedBy_Account { get; set; }
    }
}
