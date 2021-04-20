using FsInfoCat.Desktop.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Entity;
using System.Linq;
using System.Management;
using System.Security;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public sealed class SettingsViewModel : DependencyObject
    {
        private static readonly ILogger<SettingsViewModel> _logger = App.LoggerFactory.CreateLogger<SettingsViewModel>();
        private Task<HostDevice> _localMachineRegistrationTask = null;
        private const string RegisterLocalMachine_MenuItem_Text = "Register Local Machine";
        private const string UnregisterLocalMachine_MenuItem_Text = "Un-Register Local Machine";

        public event DependencyPropertyChangedEventHandler UserPropertyChanged;

        public event DependencyPropertyChangedEventHandler RolesPropertyChanged;

        public event DependencyPropertyChangedEventHandler HostDeviceRegistrationPropertyChanged;

        #region Properties

        private static readonly DependencyPropertyKey MachineSIDPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MachineSID), typeof(string), typeof(SettingsViewModel),
            new PropertyMetadata(""));

        public static readonly DependencyProperty MachineSIDProperty = MachineSIDPropertyKey.DependencyProperty;

        public string MachineSID
        {
            get { return GetValue(MachineSIDProperty) as string; }
            private set { SetValue(MachineSIDPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey MachineNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(MachineName), typeof(string), typeof(SettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty MachineNameProperty = MachineNamePropertyKey.DependencyProperty;

        public string MachineName
        {
            get { return GetValue(MachineNameProperty) as string; }
            private set { SetValue(MachineNamePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey UserPropertyKey = DependencyProperty.RegisterReadOnly(nameof(User), typeof(UserAccount), typeof(SettingsViewModel),
            new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SettingsViewModel).UserPropertyChanged?.Invoke(d, e)));

        public static readonly DependencyProperty UserProperty = UserPropertyKey.DependencyProperty;

        public UserAccount User
        {
            get { return (UserAccount)GetValue(UserProperty); }
            private set { SetValue(UserPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey RolesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Roles), typeof(UserRole), typeof(SettingsViewModel),
            new PropertyMetadata(UserRole.None, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SettingsViewModel).OnRolesPropertyChanged(e)));

        private void OnRolesPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try
            {
                RegisterLocalMachineCommand.IsEnabled = args.NewValue is UserRole role && (role.HasFlag(UserRole.ITSupport) || role.HasFlag(UserRole.Contributor));
            }
            finally { RolesPropertyChanged?.Invoke(this, args);  }
        }

        public static readonly DependencyProperty RolesProperty = RolesPropertyKey.DependencyProperty;

        public UserRole Roles
        {
            get { return (UserRole)GetValue(RolesProperty); }
            private set { SetValue(RolesPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey HostDeviceRegistrationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(HostDeviceRegistration), typeof(HostDevice), typeof(SettingsViewModel),
            new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SettingsViewModel).OnHostDeviceRegistrationPropertyChanged(e)));


        public static readonly DependencyProperty HostDeviceRegistrationProperty = HostDeviceRegistrationPropertyKey.DependencyProperty;

        public HostDevice HostDeviceRegistration
        {
            get { return (HostDevice)GetValue(HostDeviceRegistrationProperty); }
            private set { SetValue(HostDeviceRegistrationPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey RegisterLocalMachineCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RegisterLocalMachineCommand),
            typeof(Commands.RelayCommand), typeof(SettingsViewModel), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(((SettingsViewModel)d).OnRegisterLocalMachineExecute)));

        public static readonly DependencyProperty RegisterLocalMachineCommandProperty = RegisterLocalMachineCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand RegisterLocalMachineCommand => (Commands.RelayCommand)GetValue(RegisterLocalMachineCommandProperty);

        private static readonly DependencyPropertyKey RegisterLocalMachineMenuItemTextPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RegisterLocalMachineMenuItemText), typeof(string),
            typeof(MainViewModel), new PropertyMetadata(RegisterLocalMachine_MenuItem_Text));

        public static readonly DependencyProperty RegisterLocalMachineMenuItemTextProperty = RegisterLocalMachineMenuItemTextPropertyKey.DependencyProperty;

        public string RegisterLocalMachineMenuItemText
        {
            get { return GetValue(RegisterLocalMachineMenuItemTextProperty) as string; }
            private set { SetValue(RegisterLocalMachineMenuItemTextPropertyKey, value); }
        }

        #endregion

        private void OnHostDeviceRegistrationPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { RegisterLocalMachineMenuItemText = (args.NewValue is null) ? RegisterLocalMachine_MenuItem_Text : UnregisterLocalMachine_MenuItem_Text; }
            finally { HostDeviceRegistrationPropertyChanged?.Invoke(this, args); }
        }

        private void OnRegisterLocalMachineExecute(object parameter)
        {
            if (HostDeviceRegistration is null)
                RegisterLocalMachineAsync(MachineSID, MachineName);
            else
                UnregisterLocalMachineAsync(HostDeviceRegistration);
        }

        private async Task<HostDevice> CheckHostDeviceRegistrationAsync(string machineName, Action<string> setMachineSid)
        {
            SelectQuery selectQuery = new SelectQuery("SELECT * from Win32_UserAccount WHERE Name=\"Administrator\"");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery))
            {
                ManagementObjectCollection managementObjectCollection = searcher.Get();
                if (managementObjectCollection.Count > 0)
                {
                    ManagementObject item = managementObjectCollection.OfType<ManagementObject>().First();
                    SecurityIdentifier sid = new SecurityIdentifier(item["SID"] as string).AccountDomainSid;
                    if (!(sid is null))
                    {
                        string sidString = sid.ToString();
                        setMachineSid(sidString);
                        using (DbModel dbContext = new DbModel())
                            return await dbContext.HostDevices.FirstOrDefaultAsync(h => h.MachineIdentifer == sidString && h.MachineName == machineName);
                    }
                }
            }
            return null;
        }

        internal Task<HostDevice> CheckHostDeviceRegistrationAsync(bool forceRecheck)
        {
            VerifyAccess();
            Task<HostDevice> task;
            lock (this)
            {
                if ((task = _localMachineRegistrationTask) is null || (forceRecheck && task.IsCompleted))
                {
                    string machineName = Environment.MachineName.ToLower();
                    MachineName = machineName;
                    _localMachineRegistrationTask = task = CheckHostDeviceRegistrationAsync(machineName, Dispatcher.AsBeginInvocationAction<string>(sidString => MachineSID = sidString));
                }
            }

            return task;
        }

        private async Task<UserAccount> AuthenticateUserAsync(string userName, SecureString securePassword, Action<UserRole, UserAccount> onSuccess)
        {
            if (string.IsNullOrEmpty(userName) || securePassword is null || securePassword.Length == 0)
                return null;
            using (DbModel dbContext = new DbModel())
            {
                BasicLogin userCredential = (from c in dbContext.BasicLogins where c.LoginName == userName select c).FirstOrDefault();
                if (!(userCredential is null || string.IsNullOrWhiteSpace(userCredential.PwHash)) &&
                        PwHash.TryCreate(userCredential.PwHash, out PwHash? result) && result.HasValue && result.Value.Test(securePassword))
                {
                    UserAccount account = await dbContext.UserAccounts.FirstOrDefaultAsync(u => u.Id == userCredential.Id);
                    if (!(account is null))
                    {
                        onSuccess(account.GetEffectiveRoles(), account);
                        return account;
                    }
                }
            }
            return null;
        }

        internal Task<UserAccount> AuthenticateUserAsync(string userName, SecureString securePassword)
        {
            if (string.IsNullOrEmpty(userName) || securePassword is null || securePassword.Length == 0)
                return Task.FromResult<UserAccount>(null);
            return AuthenticateUserAsync(userName, securePassword, Dispatcher.AsBeginInvocationAction<UserRole, UserAccount>((roles, account) =>
            {
                User = account;
                Roles = roles;
            }));
        }

        internal Task<UserAccount> AuthenticateUserAsync_old(string userName, SecureString securePassword)
        {
            VerifyAccess();
            if (string.IsNullOrEmpty(userName) || securePassword is null || securePassword.Length == 0)
                return Task.FromResult<UserAccount>(null);
            return Task.Factory.StartNew(() =>
            {
                using (DbModel dbContext = new DbModel())
                {
                    BasicLogin userCredential = (from c in dbContext.BasicLogins where c.LoginName == userName select c).FirstOrDefault();
                    if (!(userCredential is null || string.IsNullOrWhiteSpace(userCredential.PwHash)) &&
                            PwHash.TryCreate(userCredential.PwHash, out PwHash? result) && result.HasValue && result.Value.Test(securePassword))
                    {
                        UserAccount account = (from u in dbContext.UserAccounts where u.Id == userCredential.Id select u).FirstOrDefault();
                        if (!(account is null))
                        {
                            UserRole roles = account.GetEffectiveRoles();
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                User = account;
                                Roles = roles;
                            }));
                            return account;
                        }
                    }
                }
                return null;
            });
        }

        private async Task<UserAccount> AuthenticateUserAsync(string name, SecurityIdentifier sid, Action<UserRole, UserAccount> onSuccess)
        {
            //Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    User = userAccount;
            //    Roles = roles;
            //}));
            using (DbModel dbContext = new DbModel())
            {
                string sddl = sid.AccountDomainSid.ToString();
                WindowsAuthDomain authDomain = (from d in dbContext.WindowsAuthDomains where d.SID == sddl && !d.IsInactive select d).FirstOrDefault();
                if (authDomain is null)
                    return null;
                sddl = sid.ToString();
                UserAccount userAccount = (from u in authDomain.Logins where u.SID == sddl && !u.IsInactive select u.UserAccount).FirstOrDefault();
                if (userAccount is null)
                {
                    if (authDomain.AutoAddUsers)
                    {
                        UserAccount systemUser = dbContext.GetSystemAccount();
                        using (DbContextTransaction transaction = dbContext.Database.BeginTransaction())
                        {
                            try
                            {
                                userAccount = new UserAccount
                                {
                                    CreatedOn = DateTime.Now,
                                    CreatedById = systemUser.Id,
                                    CreatedBy = systemUser,
                                    ModifiedById = systemUser.Id,
                                    ModifiedBy = systemUser,
                                    DisplayName = name,
                                    FirstName = "",
                                    LastName = name
                                };
                                userAccount.ModifiedOn = userAccount.CreatedOn;
                                dbContext.UserAccounts.Add(userAccount);
                                await dbContext.SaveChangesAsync();
                                WindowsIdentityLogin windowsIdentityLogin = new WindowsIdentityLogin
                                {
                                    Id = Guid.NewGuid(),
                                    AccountName = name,
                                    CreatedOn = DateTime.Now,
                                    CreatedById = systemUser.Id,
                                    CreatedBy = systemUser,
                                    ModifiedById = systemUser.Id,
                                    ModifiedBy = systemUser,
                                    DomainId = authDomain.Id,
                                    Domain = authDomain,
                                    SID = sddl,
                                    UserId = userAccount.Id,
                                    UserAccount = userAccount
                                };
                                windowsIdentityLogin.ModifiedOn = windowsIdentityLogin.CreatedOn;
                                userAccount.WindowsIdentityLogins.Add(windowsIdentityLogin);
                                dbContext.SaveChanges();
                                UserGroup group = authDomain.DefaultNewUserGroup;
                                if (!(group is null))
                                {
                                    userAccount.Memberships.Add(new GroupMember
                                    {
                                        AddedById = systemUser.Id,
                                        AddedBy = systemUser,
                                        AddedOn = DateTime.Now,
                                        GroupId = group.Id,
                                        Group = group,
                                        UserId = userAccount.Id,
                                        User = userAccount
                                    });
                                    await dbContext.SaveChangesAsync();
                                }
                                transaction.Commit();
                            }
                            catch
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }
                    else
                        return null;
                }
                onSuccess(userAccount.GetEffectiveRoles(), userAccount);
                return userAccount;
            }
        }

        internal Task<UserAccount> AuthenticateUserAsync(WindowsIdentity windowsIdentity)
        {
            SecurityIdentifier sid;
            if (windowsIdentity is null || windowsIdentity.IsGuest || windowsIdentity.IsAnonymous || !windowsIdentity.IsAuthenticated || (sid = windowsIdentity.User) is null || !sid.IsAccountSid())
                return Task.FromResult<UserAccount>(null);
            return AuthenticateUserAsync(windowsIdentity.Name, sid, Dispatcher.AsBeginInvocationAction<UserRole, UserAccount>((roles, userAccount) =>
            {
                User = userAccount;
                Roles = roles;
            }));
        }

        private async Task<HostDevice> ForceRegisterLocalMachineAsync(string sidString, string machineName)
        {
            using (DbModel dbContext = new DbModel())
                return await dbContext.HostDevices.FirstOrDefaultAsync(h => h.MachineIdentifer == sidString && h.MachineName == machineName);
        }

        private Task<HostDevice> RegisterLocalMachineAsync(string sidString, string machineName)
        {
            VerifyAccess();
            Task<HostDevice> task = _localMachineRegistrationTask;
            if (task is null || task.IsCompleted)
            {
                RegisterLocalMachineCommand.IsEnabled = false;
                _localMachineRegistrationTask = task = ForceRegisterLocalMachineAsync(sidString, machineName);
                task.ContinueWith(t =>
                {
                    Dispatcher.BeginInvoke(new Action(() => RegisterLocalMachineCommand.IsEnabled = true));
                    if (t.IsCanceled)
                        _logger.LogWarning("{CommandName} for registration canceled", nameof(RegisterLocalMachineCommand));
                    else if (t.IsFaulted)
                        _logger.LogError(t.Exception, "{CommandName} for registration threw an exception: {Message}", nameof(RegisterLocalMachineCommand), t.Exception.Message);
                    else
                    {
                        _logger.LogInformation("{CommandName} for registration succeeded", nameof(RegisterLocalMachineCommand));
                        Dispatcher.BeginInvoke(new Action(() => HostDeviceRegistration = task.Result));
                    }
                });
            }
            return task;
        }

        private async Task<HostDevice> ForceUnregisterLocalMachineAsync(HostDevice hostDeviceRegistration)
        {
            using (DbModel dbContext = new DbModel())
            {
                dbContext.HostDevices.Attach(hostDeviceRegistration);
                dbContext.HostDevices.Remove(hostDeviceRegistration);
                await dbContext.SaveChangesAsync();
                return null;
            }
        }

        private Task<HostDevice> UnregisterLocalMachineAsync(HostDevice hostDeviceRegistration)
        {
            VerifyAccess();
            Task<HostDevice> task = _localMachineRegistrationTask;
            if (task is null || task.IsCompleted)
            {
                RegisterLocalMachineCommand.IsEnabled = false;
                _localMachineRegistrationTask = task = ForceUnregisterLocalMachineAsync(hostDeviceRegistration);
                task.ContinueWith((Task<HostDevice> t) =>
                {
                    Dispatcher.BeginInvoke(new Action(() => RegisterLocalMachineCommand.IsEnabled = true));
                    if (t.IsCanceled)
                        _logger.LogWarning("{CommandName} for un-registration canceled", nameof(RegisterLocalMachineCommand));
                    else if (t.IsFaulted)
                        _logger.LogError(t.Exception, "{CommandName} for un-registration threw an exception: {Message}", nameof(RegisterLocalMachineCommand), t.Exception.Message);
                    else
                    {
                        _logger.LogInformation("{CommandName} for un-registration succeeded", nameof(RegisterLocalMachineCommand));
                        Dispatcher.BeginInvoke(new Action(() => HostDeviceRegistration = null));
                    }
                });
            }
            return task;
        }
    }
}
