using Backend.Repositories;
using DataAccess;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Frontend
{
    public partial class MainWindow : Window
    {
        private const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BasketballLeague560;Integrated Security=True;TrustServerCertificate=True;";

        //for ksu server
        //private const string ConnectionString = @"(localdb)\MSSQLLocalDB;";

        private ObservableCollection<EditableLeague> _leagues = new();

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
                _leagues = new ObservableCollection<EditableLeague>(
                    repo.RetrieveLeagues().Select(l => new EditableLeague(l)));
                LeagueTilesControl.ItemsSource = _leagues;
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
            if (sender is FrameworkElement fe && fe.Tag is EditableLeague league && !league.IsEditing)
            {
                ShowFrame();
                MainFrame.Navigate(new SeasonsPage(league.Model, ConnectionString));
            }
        }

        private void LeagueOptions_Click(object sender, RoutedEventArgs e)
        {
            if (!Session.IsAdmin) return;
            if (sender is Button btn)
                btn.ContextMenu.IsOpen = true;
        }

        private void EditLeague_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi &&
                mi.Parent is ContextMenu cm &&
                cm.PlacementTarget is Button btn &&
                btn.Tag is EditableLeague league)
            {
                league.IsEditing = true;
            }
        }

        private void SaveLeague_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not FrameworkElement fe || fe.Tag is not EditableLeague league) return;
            try
            {
                var executor = new SqlCommandExecutor(ConnectionString);
                var repo = new SqlLeagueRepository(executor);
                repo.UpdateLeague(league.LeagueID, league.EditName, league.Model.LocationID);
                LoadLeagues();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving league: " + ex.Message);
            }
        }

        private void CancelLeagueEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is EditableLeague league)
            {
                league.EditName = league.LeagueName;
                league.IsEditing = false;
            }
        }

        private void DeleteLeague_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not MenuItem mi ||
                mi.Parent is not ContextMenu cm ||
                cm.PlacementTarget is not Button btn ||
                btn.Tag is not EditableLeague league) return;

            var result = MessageBox.Show(
                $"Delete \"{league.LeagueName}\"? This cannot be undone.",
                "Delete League", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                var executor = new SqlCommandExecutor(ConnectionString);
                var repo = new SqlLeagueRepository(executor);
                repo.DeleteLeague(league.LeagueID);
                _leagues.Remove(league);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting league: " + ex.Message);
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
