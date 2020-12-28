using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class UserLoginRequest : IValidatableModel
    {
        private string _loginName = "";
        private string _password = "";

        [Required()]
        [MinLength(1, ErrorMessage = AppUser.Error_Message_Login_Empty)]
        [MaxLength(AppUser.Max_Length_Login_Name, ErrorMessage = AppUser.Error_Message_Login_Length)]
        [RegularExpression(ModelHelper.PATTERN_DOTTED_NAME, ErrorMessage = AppUser.Error_Message_Login_Invalid)]
        [Display(Name = AppUser.DisplayName_LoginName)]
        [DataType(DataType.Text)]
        /// <summary>
        /// Gets the user's login name.
        /// /// </summary>
        public string LoginName
        {
            get { return _loginName; }
            set { _loginName = (null == value) ? "" : value; }
        }

        public const string PropertyName_Password = "Password";
        public const string DisplayName_Password = "Password";
        public const string Error_Message_Password = "Please enter the password";
        [Required()]
        [Display(Name = DisplayName_Password)]
        [RegularExpression(@"\S+", ErrorMessage = Error_Message_Password)]
        /// <summary>
        /// Gets the hash for the user's password.
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = (null == value) ? "" : value; }
        }

        public void Normalize()
        {
            _loginName = _loginName.Trim();
        }

        private void OnValidateAll(List<ValidationResult> result)
        {
            Validate(result, AppUser.PropertyName_LoginName);
            Validate(result, PropertyName_Password);
        }

        private void Validate(List<ValidationResult> result, string propertyName)
        {
            switch (propertyName)
            {
                case PropertyName_Password:
                    if (_password.Length == 0)
                        result.Add(new ValidationResult(Error_Message_Password, new string[] { PropertyName_Password }));
                    break;
                case AppUser.PropertyName_LoginName:
                case AppUser.DisplayName_LoginName:
                    if (_loginName.Length == 0)
                        result.Add(new ValidationResult(AppUser.Error_Message_Login_Empty, new string[] { AppUser.PropertyName_LoginName }));
                    else if (_loginName.Length > AppUser.Max_Length_DisplayName)
                        result.Add(new ValidationResult(AppUser.Error_Message_Login_Length, new string[] { AppUser.PropertyName_LoginName }));
                    else if (!ModelHelper.DottedNameRegex.IsMatch(_loginName))
                        result.Add(new ValidationResult(AppUser.Error_Message_Login_Invalid, new string[] { AppUser.PropertyName_LoginName }));
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