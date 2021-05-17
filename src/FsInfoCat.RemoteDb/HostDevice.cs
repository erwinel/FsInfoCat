using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.RemoteDb
{
    public class HostDevice : IHostDevice
    {
        private string _displayName = "";
        private string _notes = "";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        internal static void BuildEntity(EntityTypeBuilder<HostDevice> builder)
        {
            builder.HasOne(d => d.Platform).WithMany(p => p.HostDevices).IsRequired();
            builder.HasOne(d => d.CreatedBy).WithMany(u => u.CreatedHostDevices).IsRequired();
            builder.HasOne(d => d.ModifiedBy).WithMany(u => u.ModifiedHostDevices).IsRequired();
            throw new NotImplementedException();
        }

        public HostDevice()
        {
            Volumes = new HashSet<Volume>();
        }

        #region Column Properties

        public Guid Id { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_DISPLAY_NAME)]
        [Required(AllowEmptyStrings = false, ErrorMessage = Constants.ERROR_MESSAGE_DISPAY_NAME_REQUIRED)]
        [MaxLength(Constants.MAX_LENGTH_DISPLAY_NAME, ErrorMessage = Constants.ERROR_MESSAGE_DISPAY_NAME_LENGTH)]
        public string DisplayName { get => _displayName; set => _displayName = value ?? ""; }

        public string MachineIdentifer { get; set; }

        public string MachineName { get; set; }

        public Guid PlatformId { get; set; }

        public string Notes { get => _notes; set => _notes = value ?? ""; }

        [DisplayName(Constants.DISPLAY_NAME_IS_INACTIVE)]
        public bool IsInactive { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_ON)]
        [Required]
        public DateTime CreatedOn { get; set; }

        public Guid CreatedById { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_ON)]
        [Required]
        public DateTime ModifiedOn { get; set; }

        public Guid ModifiedById { get; set; }

        #endregion

        #region Navigation Properties

        public HostPlatform Platform { get; set; }

        public HashSet<Volume> Volumes { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_CREATED_BY)]
        [Required]
        public UserProfile CreatedBy { get; set; }

        [DisplayName(Constants.DISPLAY_NAME_MODIFIED_BY)]
        [Required]
        public UserProfile ModifiedBy { get; set; }

        #endregion

        #region Explicit Members

        IReadOnlyCollection<IRemoteVolume> IHostDevice.Volumes => Volumes;

        IUserProfile IRemoteTimeStampedEntity.CreatedBy => CreatedBy;

        IUserProfile IRemoteTimeStampedEntity.ModifiedBy => ModifiedBy;

        IHostPlatform IHostDevice.Platform => Platform;

        #endregion
    }
}
