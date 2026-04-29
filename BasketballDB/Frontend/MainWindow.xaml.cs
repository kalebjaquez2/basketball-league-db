using Backend.Repositories;
using DataAccess;
using System.Windows;
using System.Windows.Controls;

namespace Frontend
{
    public partial class MainWindow : Window
    {
        // Example connection string - adjust to match your SSMS setup
        private const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BasketballLeague560;Integrated Security=True;TrustServerCertificate=True;";

        //for ksu server
        //private const string ConnectionString = @"(localdb)\MSSQLLocalDB;";
       // private const string ConnectionString = @"Data Source=mssql.cs.ksu.edu;Initial Catalog=kalebjaquez;User ID=kalebjaquez;Password=Ulysses#20040907;TrustServerCertificate=True;";
        public MainWindow()
        {
            InitializeComponent();
            ApplySession();
            LoadLeagues();
        }

        private void ApplySession()
        {
            UsernameLabel.Text = Session.Username;
            ManageAccountsButton.Visibility = Session.IsAdmin
                ? Visibility.Visible : Visibility.Collapsed;
            AddLeagueButton.Visibility = Session.IsAdmin
                ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ManageAccounts_Click(object sender, RoutedEventArgs e)
        {
            var win = new ManageAccountsWindow();
            win.Owner = this;
            win.ShowDialog();
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            Session.UserID = 0;
            Session.Username = "";
            Session.IsAdmin = false;

            // Close this window first, then show login as a proper dialog
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Close();

            var login = new LoginWindow();
            if (login.ShowDialog() == true)
            {
                Application.Current.ShutdownMode = ShutdownMode.OnLastWindowClose;
                new MainWindow().Show();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void LoadLeagues()
        {
            try
            {
                var executor = new SqlCommandExecutor(ConnectionString);
                var repo = new SqlLeagueRepository(executor);

                // Use the backend to fetch all leagues
                LeagueTilesControl.ItemsSource = repo.RetrieveLeagues();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not load leagues: {ex.Message}");
            }
        }

        private void ShowHomeScreen()
        {
            HomeScreen.Visibility = Visibility.Visible;
            MainFrame.Visibility = Visibility.Collapsed;
        }

        private void ShowFrame()
        {
            HomeScreen.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
        }

        private void AddLeague_Click(object sender, RoutedEventArgs e)
        {
            // 1. Create the backend infrastructure
            var executor = new SqlCommandExecutor(ConnectionString);
            var repo = new SqlLeagueRepository(executor);

            // 2. Pass the repo into the dialog
            var dialog = new AddLeagueDialog(repo);
            dialog.Owner = this;

            if (dialog.ShowDialog() == true)
            {
                // 3. Success! Refresh the list
                LoadLeagues();
            }
        }

        private void LeagueTile_Click(object sender, RoutedEventArgs e)
        {
            // 1. Get the League object from the clicked button's DataContext
            // This is much more reliable than checking for a Tag
            if (sender is FrameworkElement element && element.DataContext is Backend.Models.League selectedLeague)
            {
                // 2. Show the Frame (hides the league list)
                ShowFrame();

                // 3. Create the SeasonsPage and navigate to it
                // We pass the whole league object and the connection string
                var seasonsPage = new SeasonsPage(selectedLeague, ConnectionString);
                MainFrame.Navigate(seasonsPage);
            }
            else
            {
                // If this hits, the XAML isn't passing the DataContext correctly
                MessageBox.Show("Error: League data not found on the clicked tile.");
            }
        }

        private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void MaxRestore_Click(object sender, RoutedEventArgs e) =>
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

        private void CloseWindow_Click(object sender, RoutedEventArgs e) => Close();

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            MaxRestoreBtn.Content = WindowState == WindowState.Maximized ? "❐" : "□";
        }
    }
}
