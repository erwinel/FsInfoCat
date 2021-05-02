﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class RemoteDbContainer : DbContext
    {
        public RemoteDbContainer()
            : base("name=RemoteDbContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<Volume> Volumes { get; set; }
        public virtual DbSet<Directory> Directories { get; set; }
        public virtual DbSet<File> Files { get; set; }
        public virtual DbSet<FileSystem> FileSystems { get; set; }
        public virtual DbSet<HostDevice> HostDevices { get; set; }
        public virtual DbSet<HostPlatform> HostPlatforms { get; set; }
        public virtual DbSet<FsSymbolicName> FsSymbolicNames { get; set; }
        public virtual DbSet<HashCalculation> HashCalculations { get; set; }
        public virtual DbSet<Redundancy> Redundancies { get; set; }
        public virtual DbSet<Comparison> Comparisons { get; set; }
        public virtual DbSet<FileRelocateTask> FileRelocateTasks { get; set; }
        public virtual DbSet<DirectoryRelocateTask> DirectoryRelocateTasks { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
    }
}