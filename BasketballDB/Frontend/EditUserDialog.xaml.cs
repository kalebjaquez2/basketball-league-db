using System;
using System.Windows;
using Backend.Models;
using Backend.Repositories;
using DataAccess;

namespace Frontend
{
    public partial class EditUserDialog : Window
    {
        private readonly User _user;

        public EditUserDialog(User user)
        {
            InitializeComponent();
            _user = user;
            TitleLabel.Text = $"EDIT USER — {user.Username.ToUpper()}";
            UsernameBox.Text = user.Username;
            UsernameBox.Focus();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Visibility = Visibility.Collapsed;

            string newUsername = UsernameBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(newUsername))
            {
                ShowError("Username cannot be empty.");
                return;
            }

            string newPassword = NewPasswordBox.Password;
            string confirm = ConfirmPasswordBox.Password;

            if (!string.IsNullOrEmpty(newPassword) && newPassword != confirm)
            {
                ShowError("Passwords do not match.");
                return;
            }

            string? passwordHash = string.IsNullOrEmpty(newPassword)
                ? null
                : LoginWindow.HashPassword(newPassword);

            try
            {
                var executor = new SqlCommandExecutor(Session.ConnectionString);
                var repo = new SqlUserRepository(executor);
                repo.UpdateUserCredentials(_user.UserID, newUsername, passwordHash);

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
