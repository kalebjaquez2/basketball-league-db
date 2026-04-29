using System;
using System.Windows;
using Backend.Repositories;
using DataAccess;

namespace Frontend
{
    public partial class CreateAccountDialog : Window
    {
        public CreateAccountDialog()
        {
            InitializeComponent();
            UsernameBox.Focus();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Visibility = Visibility.Collapsed;

            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password;
            string confirm = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowError("Username is required.");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowError("Password is required.");
                return;
            }

            if (password != confirm)
            {
                ShowError("Passwords do not match.");
                return;
            }

            try
            {
                var executor = new SqlCommandExecutor(Session.ConnectionString);
                var repo = new SqlUserRepository(executor);
                repo.CreateUser(username, LoginWindow.HashPassword(password), isAdmin: false);

                MessageBox.Show("Account created! You can now log in.", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                ShowError("Error: " + ex.Message);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ShowError(string msg)
        {
            ErrorMessage.Text = msg;
            ErrorMessage.Visibility = Visibility.Visible;
        }
    }
}
