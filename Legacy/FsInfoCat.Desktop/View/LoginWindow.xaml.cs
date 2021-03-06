using System;
using System.Windows;

namespace FsInfoCat.Desktop.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        //private void LoginButton_Click(object sender, RoutedEventArgs e)
        //{
        //    UserNameTextBox.IsEnabled = PasswordTextBox.IsEnabled = LoginButton.IsEnabled = CancelButton.IsEnabled = false;
        //    ViewModel.SettingsViewModel settingsViewModel = App.GetSettingsVM();
        //    App.GetSettingsVM().AuthenticateUserAsync(UserNameTextBox.Text, PasswordTextBox.SecurePassword).ContinueWith(t =>
        //    {
        //        if (t.IsCanceled)
        //            OnLoginFailed(null);
        //        else if (t.IsFaulted)
        //            OnLoginFailed(t.Exception);
        //        else
        //            OnLoginCompleted(t.Result);
        //    });
        //}

        //private void OnLoginCompleted(UserAccount result)
        //{
        //    if (result is null)
        //    {
        //        ErrorMessageTextBlock.Text = "Invalid user name or password";
        //        ErrorMessageTextBlock.Visibility = Visibility.Visible;
        //        UserNameTextBox.IsEnabled = PasswordTextBox.IsEnabled = LoginButton.IsEnabled = CancelButton.IsEnabled = true;
        //    }
        //    else
        //    {
        //        DialogResult = true;
        //        Close();
        //    }
        //}

        //private void OnLoginFailed(AggregateException exception)
        //{
        //    ErrorMessageTextBlock.Text = $"Error validating user name or password: {(string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message)}";
        //    ErrorMessageTextBlock.Visibility = Visibility.Visible;
        //    UserNameTextBox.IsEnabled = PasswordTextBox.IsEnabled = LoginButton.IsEnabled = CancelButton.IsEnabled = true;
        //}

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void LoginViewModel_LoginSucceeded(object sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void LoginViewModel_LoginAborted(object sender, EventArgs e)
        {

        }
    }
}
