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
    
    [Flags]
    public enum UserRole : byte
    {
        None = 0,
        Reader = 1,
        Auditor = 2,
        Contributor = 4,
        ChangeAdministrator = 8,
        AppAdministrator = 16,
        SystemAdmin = 32,
        ITSupport = 64
    }
}
