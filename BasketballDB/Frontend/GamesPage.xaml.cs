using System;
using System.Windows;
using System.Windows.Controls;
using Backend.Models;
using Backend.Repositories;
using DataAccess;

namespace Frontend
{
    public partial class GamesPage : Page
    {
        private readonly Team _team;
        private readonly string _connectionString;
        private readonly int _seasonId;

        public GamesPage(Team team, string connectionString)
        {
            InitializeComponent();
            _team = team;
            _connectionString = connectionString;
            _seasonId = team.SeasonID;

            // Set the header text
            PageTitle.Text = $"{_team.TeamName.ToUpper()} SCHEDULE";

            LoadGames();
        }

        private void LoadGames()
        {
            try
            {
                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlGameRepository(executor);

                // Retrieve games where this team is either Home or Away
                var games = repo.RetrieveGamesByTeam(_team.TeamID);
                GamesList.ItemsSource = games;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading games: " + ex.Message);
            }
        }

        // This is the method the XAML was looking for!
        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddGameDialog(_seasonId, _connectionString);
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true)
            {
                LoadGames(); // Refresh the list after the new game is saved
            }
        }

        private void GameTile_Click(object sender, RoutedEventArgs e)
        {
            // Get the data from the button's context
            if (sender is Button btn && btn.DataContext is Game selectedGame)
            {
                this.NavigationService.Navigate(new PlayerGameStatsPage(selectedGame, _connectionString));
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}