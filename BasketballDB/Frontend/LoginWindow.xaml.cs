using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Backend.Repositories;
using DataAccess;

namespace Frontend
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            UsernameBox.Focus();
        }

        private void Login_Click(object sender, RoutedEventArgs e) => AttemptLogin();

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) AttemptLogin();
        }

        private void AttemptLogin()
        {
            ErrorMessage.Visibility = Visibility.Collapsed;

            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password))
            {
                ShowError("Username and password are required.");
                return;
            }

            try
            {
                var executor = new SqlCommandExecutor(Session.ConnectionString);
                var repo = new SqlUserRepository(executor);
                var user = repo.FetchUserByCredentials(username, HashPassword(password));

                if (user == null)
                {
                    ShowError("Invalid username or password.");
                    return;
                }

                Session.UserID = user.UserID;
                Session.Username = user.Username;
                Session.IsAdmin = user.IsAdmin;

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                ShowError("Connection error: " + ex.Message);
            }
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CreateAccountDialog();
            dialog.Owner = this;
            dialog.ShowDialog();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void ShowError(string msg)
        {
            ErrorMessage.Text = msg;
            ErrorMessage.Visibility = Visibility.Visible;
        }

        internal static string HashPassword(string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes).ToLower();
        }
    }
}
