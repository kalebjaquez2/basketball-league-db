using System;
using System.Windows;
using System.Windows.Controls;
using Backend.Models;
using Backend.Repositories;
using DataAccess;

namespace Frontend
{
    public partial class TeamsPage : Page
    {
        private readonly Season _season;
        private readonly string _connectionString;

        public TeamsPage(Season season, string connectionString)
        {
            InitializeComponent();
            _season = season;
            _connectionString = connectionString;

            // Set the header to show which season we are in
            SeasonHeader.Text = $"{_season.StartDate:yyyy} Season";
            LoadTeams();
        }

        private void LoadTeams()
        {
            var executor = new SqlCommandExecutor(_connectionString);
            var repo = new SqlTeamRepository(executor);

            // Retrieve teams for this specific season
            var teams = repo.RetrieveTeamsBySeason(_season.SeasonID);

            // Crucial: Assign the collection to the Container, not the ItemsControl
            TeamsDataContainer.Collection = teams;
        }

        private void AddTeam_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddTeamDialog(_season.SeasonID, _connectionString);
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true)
            {
                // Refresh list after adding
                LoadTeams();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            // Pass SeasonID and connection string to the dialog
            var dialog = new AddGameDialog(_season.SeasonID, _connectionString);
            dialog.Owner = Window.GetWindow(this);


            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show("Game scheduled successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void TeamTile_Click(object sender, RoutedEventArgs e)
        {
            // Get the team data from the clicked button's context
            if (sender is Button btn && btn.DataContext is Team selectedTeam)
            {
                // Navigate to the GamesPage, passing the team and connection string
                var gamesPage = new GamesPage(selectedTeam, _connectionString);
                this.NavigationService.Navigate(gamesPage);
            }
        }
        private void Roster_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Team selectedTeam)
            {
                var rosterPage = new RosterPage(selectedTeam, _connectionString);
                this.NavigationService.Navigate(rosterPage);
            }
        }
    }
}