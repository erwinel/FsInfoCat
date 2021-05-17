using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class DirectoryRelocateTask : IDirectoryRelocateTask
    {
        public DirectoryRelocateTask()
        {
            SourceDirectories = new HashSet<FsDirectory>();
        }

        public Guid Id { get; set; }

        public AppTaskStatus Status { get; set; }

        public PriorityLevel Priority { get; set; }

        private string _shortDescription = "";

        [DisplayName(Constants.DISPLAY_NAME_SHORT_DESCRIPTION)]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_SHORT_DESCRIPTION)]
        [MaxLength(Constants.MAX_LENGTH_SHORT_DESCRIPTION)]
        public string ShortDescription { get => _shortDescription; set => _shortDescription = value ?? ""; }

        private string _notes = "";

        public string Notes { get => _notes; set => _notes = value ?? ""; }

        public bool IsInactive { get; set; }

        public Guid TargetDirectoryId { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime CreatedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        public DateTime ModifiedOn { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_SOURCE_DIRECTORIES)]
        public virtual HashSet<FsDirectory> SourceDirectories { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_TARGET_DIRECTORY)]
        [Required(ErrorMessage = Constants.ERROR_MESSAGE_TARGET_DIRECTORY)]
        public virtual FsDirectory TargetDirectory { get; set; }

        public Guid? AssignmentGroupId { get; set; }

        public Guid? AssignedToId { get; set; }

        IReadOnlyCollection<IRemoteSubDirectory> IDirectoryRelocateTask.SourceDirectories => SourceDirectories;

        public UserGroup AssignmentGroup { get; set; }

        public UserProfile AssignedTo { get; set; }

        IRemoteSubDirectory IDirectoryRelocateTask.TargetDirectory => TargetDirectory;

        public Guid CreatedById { get; set; }

        public Guid ModifiedById { get; set; }

        public UserProfile CreatedBy { get; set; }

        public UserProfile ModifiedBy { get; set; }

        IUserGroup IDirectoryRelocateTask.AssignmentGroup => AssignmentGroup;

        IUserProfile IDirectoryRelocateTask.AssignedTo => AssignedTo;

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => ModifiedBy;

        internal static void BuildEntity(EntityTypeBuilder<DirectoryRelocateTask> builder)
        {
            builder.HasKey(nameof(Id));
            builder.Property(nameof(ShortDescription)).HasMaxLength(Constants.MAX_LENGTH_SHORT_DESCRIPTION).IsRequired();
            builder.Property(nameof(Notes)).HasDefaultValue("");
            builder.HasOne(p => p.TargetDirectory).WithMany(d => d.TargetDirectoryRelocationTasks).IsRequired();
            builder.HasOne(t => t.AssignmentGroup).WithMany(u => u.DirectoryRelocationTasks);
            builder.HasOne(t => t.AssignedTo).WithMany(u => u.DirectoryRelocationTasks);
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedDirectoryRelocateTasks).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedDirectoryRelocateTasks).IsRequired();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateProperty(ShortDescription, new ValidationContext(this, null, null) { MemberName = nameof(ShortDescription) }, results);
            Validator.TryValidateProperty(TargetDirectory, new ValidationContext(this, null, null) { MemberName = nameof(TargetDirectory) }, results);
            if (CreatedOn.CompareTo(ModifiedOn) > 0)
                results.Add(new ValidationResult(Constants.ERROR_MESSAGE_MODIFIED_ON, new string[] { nameof(ModifiedOn) }));
            return results;
        }
    }
}
