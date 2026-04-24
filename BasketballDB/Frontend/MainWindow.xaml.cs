using Backend.Models;
using Backend.Repositories;
using DataAccess;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Frontend
{
    public partial class MainWindow : Window
    {
        private const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDb;Initial Catalog=BasketballLeague560;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=""SQL Server Management Studio"";Command Timeout=0";

        public MainWindow()
        {
            InitializeComponent();
            LoadLeagues();
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
            // Create backend infrastructure
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
            if (sender is FrameworkElement element && element.DataContext is League selectedLeague)
            {
                // Create the page
                var seasonsPage = new SeasonsPage(selectedLeague, ConnectionString);

                // Hide the Home Screen (
                HomeScreen.Visibility = Visibility.Collapsed;

                // Show the Frame and Navigate
                MainFrame.Visibility = Visibility.Visible;
                MainFrame.Navigate(seasonsPage);
            }
        }


    }
}
