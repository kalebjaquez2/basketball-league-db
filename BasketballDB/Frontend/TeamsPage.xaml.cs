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
            SeasonHeader.Text = $"{_season.StartDate:yyyy} Season";
            LoadTeams();
            LoadMostActivePlayers();
        }

        private void LoadTeams()
        {
            try
            {
                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlTeamRepository(executor);
                var teams = repo.RetrieveTeamsBySeason(_season.SeasonID);
                TeamsDataContainer.Collection = teams;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading teams: " + ex.Message);
            }
        }

        private void LoadMostActivePlayers()
        {
            try
            {
                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlStatsRepository(executor);
                MostActivePlayersList.ItemsSource =
                    repo.RetrieveMostActivePlayers(_season.SeasonID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading most active players: " + ex.Message);
            }
        }

        private void AddTeam_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddTeamDialog(_season.SeasonID, _connectionString);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                LoadTeams();
                LoadMostActivePlayers();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddGameDialog(_season.SeasonID, _connectionString);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                MessageBox.Show("Game scheduled successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void TeamTile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Team selectedTeam)
            {
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