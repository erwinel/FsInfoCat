using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class UserProfile : IUserProfile
    {
        private string _displayName = "";
        private string _notes = "";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        internal static void BuildEntity(EntityTypeBuilder<UserProfile> builder)
        {
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedUserProfiles).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedUserProfiles).IsRequired();
            throw new NotImplementedException();
        }

        public UserProfile()
        {
            AssignmentGroups = new HashSet<UserGroup>();
            DirectoryRelocationTasks = new HashSet<DirectoryRelocateTask>();
            FileRelocationTasks = new HashSet<FileRelocateTask>();
            CreatedSymbolicNames = new HashSet<SymbolicName>();
            CreatedComparisons = new HashSet<FileComparison>();
            CreatedDirectories = new HashSet<FsDirectory>();
            CreatedFiles = new HashSet<FsFile>();
            CreatedFileSystems = new HashSet<FileSystem>();
            CreatedHashCalculations = new HashSet<HashCalculation>();
            CreatedHostDevices = new HashSet<HostDevice>();
            CreatedHostPlatforms = new HashSet<HostPlatform>();
            CreatedRedundancies = new HashSet<Redundancy>();
            CreatedUserProfiles = new HashSet<UserProfile>();
            CreatedVolumes = new HashSet<Volume>();
            CreatedDirectoryRelocateTasks = new HashSet<DirectoryRelocateTask>();
            CreatedFileRelocateTasks = new HashSet<FileRelocateTask>();
            CreatedUserGroups = new HashSet<UserGroup>();
            ModifiedSymbolicNames = new HashSet<SymbolicName>();
            ModifiedComparisons = new HashSet<FileComparison>();
            ModifiedDirectories = new HashSet<FsDirectory>();
            ModifiedFiles = new HashSet<FsFile>();
            ModifiedFileSystems = new HashSet<FileSystem>();
            ModifiedHashCalculations = new HashSet<HashCalculation>();
            ModifiedHostDevices = new HashSet<HostDevice>();
            ModifiedHostPlatforms = new HashSet<HostPlatform>();
            ModifiedRedundancies = new HashSet<Redundancy>();
            ModifiedUserProfiles = new HashSet<UserProfile>();
            ModifiedVolumes = new HashSet<Volume>();
            ModifiedDirectoryRelocateTasks = new HashSet<DirectoryRelocateTask>();
            ModifiedFileRelocateTasks = new HashSet<FileRelocateTask>();
            ModifiedUserGroups = new HashSet<UserGroup>();
        }

        #region Column Properties

        public Guid Id { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_DisplayName), ResourceType = typeof(ModelResources))]
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_DisplayNameRequired), ErrorMessageResourceType = typeof(ModelResources))]
        [LengthValidationDbSettings(nameof(DBSettings.DbColMaxLen_DisplayName), ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_NameLength), ErrorMessageResourceType = typeof(ModelResources))]
        public string DisplayName { get => _displayName; set => _displayName = value ?? ""; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MI { get; set; }

        public string Suffix { get; set; }

        public string Title { get; set; }

        public int? DbPrincipalId { get; set; }

        public byte[] SID { get; set; }

        public string LoginName { get; set; }

        public UserRole ExplicitRoles { get; set; }

        public string Notes { get => _notes; set => _notes = value ?? ""; }

        public bool IsInactive { get; set; }

                [Required]
        [Display(Name = nameof(ModelResources.DisplayName_CreatedOn), ResourceType = typeof(ModelResources))]
        public DateTime CreatedOn { get; set; }

        public Guid CreatedById { get; set; }

                [Required]
        [Display(Name = nameof(ModelResources.DisplayName_ModifiedOn), ResourceType = typeof(ModelResources))]
        public DateTime ModifiedOn { get; set; }

        public Guid ModifiedById { get; set; }

        #endregion

        #region Navigation Properties

        public HashSet<UserGroup> AssignmentGroups { get; set; }

        public HashSet<DirectoryRelocateTask> DirectoryRelocationTasks { get; set; }

        public HashSet<FileRelocateTask> FileRelocationTasks { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_CreatedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_CreatedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile CreatedBy { get; set; }

        [Display(Name = nameof(ModelResources.DisplayName_ModifiedBy), ResourceType = typeof(ModelResources))]
        [Required(ErrorMessageResourceName = nameof(ModelResources.ErrorMessage_ModifiedBy), ErrorMessageResourceType = typeof(ModelResources))]
        public UserProfile ModifiedBy { get; set; }

        public HashSet<SymbolicName> CreatedSymbolicNames { get; set; }

        public HashSet<FileComparison> CreatedComparisons { get; set; }

        public HashSet<FsDirectory> CreatedDirectories { get; set; }

        public HashSet<FsFile> CreatedFiles { get; set; }

        public HashSet<FileSystem> CreatedFileSystems { get; set; }

        public HashSet<HashCalculation> CreatedHashCalculations { get; set; }

        public HashSet<HostDevice> CreatedHostDevices { get; set; }

        public HashSet<HostPlatform> CreatedHostPlatforms { get; set; }

        public HashSet<Redundancy> CreatedRedundancies { get; set; }

        public HashSet<UserProfile> CreatedUserProfiles { get; set; }

        public HashSet<Volume> CreatedVolumes { get; set; }

        public HashSet<Volume> ModifiedVolumes { get; set; }

        public HashSet<UserProfile> ModifiedUserProfiles { get; set; }

        public HashSet<Redundancy> ModifiedRedundancies { get; set; }

        public HashSet<HostPlatform> ModifiedHostPlatforms { get; set; }

        public HashSet<HostDevice> ModifiedHostDevices { get; set; }

        public HashSet<HashCalculation> ModifiedHashCalculations { get; set; }

        public HashSet<SymbolicName> ModifiedSymbolicNames { get; set; }

        public HashSet<FileSystem> ModifiedFileSystems { get; set; }

        public HashSet<FsFile> ModifiedFiles { get; set; }

        public HashSet<FsDirectory> ModifiedDirectories { get; set; }

        public HashSet<FileComparison> ModifiedComparisons { get; set; }

        public HashSet<DirectoryRelocateTask> CreatedDirectoryRelocateTasks { get; set; }

        public HashSet<FileRelocateTask> CreatedFileRelocateTasks { get; set; }

        public HashSet<UserGroup> CreatedUserGroups { get; set; }

        public HashSet<DirectoryRelocateTask> ModifiedDirectoryRelocateTasks { get; set; }

        public HashSet<FileRelocateTask> ModifiedFileRelocateTasks { get; set; }

        public HashSet<UserGroup> ModifiedUserGroups { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<byte> IUserProfile.SID => SID;

        IReadOnlyCollection<IRemoteSymbolicName> IUserProfile.CreatedSymbolicNames => CreatedSymbolicNames;

        IReadOnlyCollection<IRemoteFileComparison> IUserProfile.CreatedComparisons => CreatedComparisons;

        IReadOnlyCollection<IRemoteSubDirectory> IUserProfile.CreatedDirectories => CreatedDirectories;

        IReadOnlyCollection<IRemoteFile> IUserProfile.CreatedFiles => CreatedFiles;

        IReadOnlyCollection<IRemoteFileSystem> IUserProfile.CreatedFileSystems => CreatedFileSystems;

        IReadOnlyCollection<IRemoteHashCalculation> IUserProfile.CreatedHashCalculations => CreatedHashCalculations;

        IReadOnlyCollection<IHostDevice> IUserProfile.CreatedHostDevices => CreatedHostDevices;

        IReadOnlyCollection<IHostPlatform> IUserProfile.CreatedHostPlatforms => CreatedHostPlatforms;

        IReadOnlyCollection<IRemoteRedundancy> IUserProfile.CreatedRedundancies => CreatedRedundancies;

        IReadOnlyCollection<IUserProfile> IUserProfile.CreatedUserProfiles => CreatedUserProfiles;

        IReadOnlyCollection<IRemoteVolume> IUserProfile.CreatedVolumes => CreatedVolumes;

        IReadOnlyCollection<IRemoteVolume> IUserProfile.ModifiedVolumes => ModifiedVolumes;

        IReadOnlyCollection<IUserProfile> IUserProfile.ModifiedUserProfiles => ModifiedUserProfiles;

        IReadOnlyCollection<IRemoteRedundancy> IUserProfile.ModifiedRedundancies => ModifiedRedundancies;

        IReadOnlyCollection<IHostPlatform> IUserProfile.ModifiedHostPlatforms => ModifiedHostPlatforms;

        IReadOnlyCollection<IHostDevice> IUserProfile.ModifiedHostDevices => ModifiedHostDevices;

        IReadOnlyCollection<IRemoteHashCalculation> IUserProfile.ModifiedHashCalculations => ModifiedHashCalculations;

        IReadOnlyCollection<IRemoteSymbolicName> IUserProfile.ModifiedSymbolicNames => ModifiedSymbolicNames;

        IReadOnlyCollection<IRemoteFileSystem> IUserProfile.ModifiedFileSystems => ModifiedFileSystems;

        IReadOnlyCollection<IRemoteFile> IUserProfile.ModifiedFiles => ModifiedFiles;

        IReadOnlyCollection<IRemoteSubDirectory> IUserProfile.ModifiedDirectories => ModifiedDirectories;

        IReadOnlyCollection<IRemoteFileComparison> IUserProfile.ModifiedComparisons => ModifiedComparisons;

        IReadOnlyCollection<IUserGroup> IUserProfile.AssignmentGroups => AssignmentGroups;

        IReadOnlyCollection<IDirectoryRelocateTask> IUserProfile.DirectoryRelocationTasks => DirectoryRelocationTasks;

        IReadOnlyCollection<IFileRelocateTask> IUserProfile.FileRelocationTasks => FileRelocationTasks;

        IReadOnlyCollection<IDirectoryRelocateTask> IUserProfile.CreatedDirectoryRelocateTasks => CreatedDirectoryRelocateTasks;

        IReadOnlyCollection<IFileRelocateTask> IUserProfile.CreatedFileRelocateTasks => CreatedFileRelocateTasks;

        IReadOnlyCollection<IUserGroup> IUserProfile.CreatedUserGroups => CreatedUserGroups;

        IReadOnlyCollection<IDirectoryRelocateTask> IUserProfile.ModifiedDirectoryRelocateTasks => ModifiedDirectoryRelocateTasks;

        IReadOnlyCollection<IFileRelocateTask> IUserProfile.ModifiedFileRelocateTasks => ModifiedFileRelocateTasks;

        IReadOnlyCollection<IUserGroup> IUserProfile.ModifiedUserGroups => ModifiedUserGroups;

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => ModifiedBy;

        #endregion
    }
}
