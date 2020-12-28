using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FsInfoCat.Models.DB;

namespace FsInfoCat.Models
{
    public class HostDeviceRegRequest : IHostDeviceReg
    {
        private string _displayName = "";
        private string _machineIdentifer = "";
        private string _machineName = "";

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (null == value) ? "" : value; }
        }

        public string MachineIdentifer
        {
            get { return _machineIdentifer; }
            set { _machineIdentifer = (null == value) ? "" : value; }
        }

        public string MachineName
        {
            get { return _machineName; }
            set { _machineName = (null == value) ? "" : value; }
        }
        public bool IsWindows { get; set; }

        Guid IHostDeviceReg.HostID { get => Guid.Empty; set => throw new NotSupportedException(); }

        public void Normalize()
        {
            _machineIdentifer = ModelHelper.CoerceAsWsNormalized(_machineIdentifer);
            _machineName = _machineName.Trim();
            if ((_displayName = ModelHelper.CoerceAsWsNormalized(_displayName)).Length == 0)
                _displayName = _machineName;
        }

        private void OnValidateAll(List<ValidationResult> result)
        {
            Validate(result, HostDevice.PropertyName_DisplayName);
            Validate(result, HostDevice.PropertyName_MachineIdentifer);
            Validate(result, HostDevice.PropertyName_MachineName);
        }

        private void Validate(List<ValidationResult> result, string propertyName)
        {
            switch (propertyName)
            {
                case HostDevice.PropertyName_MachineIdentifer:
                case HostDevice.DisplayName_MachineIdentifer:
                    if (_machineIdentifer.Length > HostDevice.Max_Length_DisplayName)
                        result.Add(new ValidationResult(HostDevice.Error_Message_MachineIdentifer, new string[] { HostDevice.PropertyName_MachineIdentifer }));
                    break;
                case HostDevice.PropertyName_MachineName:
                case HostDevice.DisplayName_MachineName:
                    if (_machineName.Length == 0)
                        result.Add(new ValidationResult(HostDevice.Error_Message_MachineName_Empty, new string[] { HostDevice.PropertyName_MachineName }));
                    else if (_machineName.Length > HostDevice.Max_Length_DisplayName)
                        result.Add(new ValidationResult(HostDevice.Error_Message_MachineName_Length, new string[] { HostDevice.PropertyName_MachineName }));
                    else if (!ModelHelper.MachineNameRegex.IsMatch(_machineName))
                        result.Add(new ValidationResult(HostDevice.Error_Message_MachineName_Invalid, new string[] { HostDevice.PropertyName_MachineName }));
                    break;
            }
        }

        public IList<ValidationResult> ValidateAll()
        {
            List<ValidationResult> result = new List<ValidationResult>();
            Normalize();
            OnValidateAll(result);
            return result;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> result = new List<ValidationResult>();
            Normalize();
            if (string.IsNullOrEmpty(validationContext.DisplayName))
                OnValidateAll(result);
            else
                Validate(result, validationContext.DisplayName);
            return result;
        }
    }
}
