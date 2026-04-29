using System;
using System.Windows;
using System.Windows.Controls;
using Backend.Models;
using Backend.Repositories;
using DataAccess;

namespace Frontend
{
    public partial class ManageAccountsWindow : Window
    {
        public ManageAccountsWindow()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                var executor = new SqlCommandExecutor(Session.ConnectionString);
                var repo = new SqlUserRepository(executor);
                UsersList.ItemsSource = repo.RetrieveUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }

        private void AdminCheck_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not CheckBox cb || cb.DataContext is not User user)
                return;

            if (cb.IsChecked == false)
            {
                // Protect the built-in admin account and the currently logged-in user
                if (user.Username == "admin")
                {
                    cb.IsChecked = true;
                    MessageBox.Show("The built-in 'admin' account cannot have admin access removed.",
                        "Protected Account", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (user.UserID == Session.UserID)
                {
                    cb.IsChecked = true;
                    MessageBox.Show("You cannot remove your own admin access.",
                        "Not Allowed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            try
            {
                var executor = new SqlCommandExecutor(Session.ConnectionString);
                var repo = new SqlUserRepository(executor);
                repo.UpdateUserAdminStatus(user.UserID, cb.IsChecked == true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating admin status: " + ex.Message);
                cb.IsChecked = !cb.IsChecked;
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.DataContext is not User user)
                return;

            var dialog = new EditUserDialog(user);
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
                LoadUsers();
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddErrorMessage.Visibility = Visibility.Collapsed;

            string username = NewUsernameBox.Text.Trim();
            string password = NewPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowAddError("Username is required.");
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowAddError("Password is required.");
                return;
            }

            bool isAdmin = GrantAdminCheck.IsChecked == true;

            try
            {
                var executor = new SqlCommandExecutor(Session.ConnectionString);
                var repo = new SqlUserRepository(executor);
                repo.CreateUser(username, LoginWindow.HashPassword(password), isAdmin);

                NewUsernameBox.Clear();
                NewPasswordBox.Clear();
                GrantAdminCheck.IsChecked = false;
                LoadUsers();
            }
            catch (Exception ex)
            {
                ShowAddError("Error: " + ex.Message);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();

        private void ShowAddError(string msg)
        {
            AddErrorMessage.Text = msg;
            AddErrorMessage.Visibility = Visibility.Visible;
        }
    }
}
