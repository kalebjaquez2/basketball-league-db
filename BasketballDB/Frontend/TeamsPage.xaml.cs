using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        private ObservableCollection<EditableTeam> _teams = new();

        public TeamsPage(Season season, string connectionString)
        {
            InitializeComponent();
            _season = season;
            _connectionString = connectionString;
            SeasonHeader.Text = $"{_season.StartDate:yyyy} Season";
            LoadTeams();
            LoadStandings();
            LoadMostActivePlayers();
            if (Session.IsAdmin)
                AddTeamButton.Visibility = Visibility.Visible;
        }

        private void LoadTeams()
        {
            try
            {
                var executor = new SqlCommandExecutor(_connectionString);
                var teamRepo = new SqlTeamRepository(executor);
                var statsRepo = new SqlStatsRepository(executor);
                var teams = teamRepo.RetrieveTeamsBySeason(_season.SeasonID);
                var performance = statsRepo.RetrieveTeamPerformance(_season.SeasonID);
                var combined = teams
                    .Select(t => new TeamWithPerformance(t, performance.FirstOrDefault(p => p.TeamID == t.TeamID)))
                    .Select(t => new EditableTeam(t))
                    .ToList();
                _teams = new ObservableCollection<EditableTeam>(combined);
                TeamsList.ItemsSource = _teams;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading teams: " + ex.Message);
            }
        }

        private void LoadStandings()
        {
            try
            {
                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlStatsRepository(executor);
                StandingsList.ItemsSource = repo.RetrieveTeamPerformance(_season.SeasonID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading standings: " + ex.Message);
            }
        }

        private void LoadMostActivePlayers()
        {
            try
            {
                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlStatsRepository(executor);
                MostActivePlayersList.ItemsSource = repo.RetrieveMostActivePlayers(_season.SeasonID);
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
                LoadStandings();
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
                MessageBox.Show("Game scheduled successfully!", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void TeamTile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is EditableTeam et)
            {
                var team = new Team(et.TeamID, et.SeasonID, et.TeamName);
                this.NavigationService.Navigate(new GamesPage(team, _connectionString));
            }
        }

        private void Roster_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is EditableTeam et)
            {
                var team = new Team(et.TeamID, et.SeasonID, et.TeamName);
                this.NavigationService.Navigate(new RosterPage(team, _connectionString));
            }
        }

        private void TeamOptions_Click(object sender, RoutedEventArgs e)
        {
            if (!Session.IsAdmin) return;

            if (sender is Button btn)
            {
                btn.ContextMenu.PlacementTarget = btn;
                btn.ContextMenu.IsOpen = true;
                e.Handled = true;
            }
        }

        private void EditTeam_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi &&
                mi.Parent is ContextMenu cm &&
                cm.PlacementTarget is Button btn &&
                btn.Tag is EditableTeam team)
            {
                team.IsEditing = true;
            }
        }

        private void SaveTeam_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is EditableTeam team)
            {
                try
                {
                    var executor = new SqlCommandExecutor(_connectionString);
                    var repo = new SqlTeamRepository(executor);
                    repo.UpdateTeam(team.TeamID, team.EditName.Trim());
                    LoadTeams();
                    LoadStandings();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving team: " + ex.Message);
                    team.IsEditing = false;
                }
            }
        }

        private void CancelTeamEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is EditableTeam team)
                team.IsEditing = false;
        }

        private void DeleteTeam_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi &&
                mi.Parent is ContextMenu cm &&
                cm.PlacementTarget is Button btn &&
                btn.Tag is EditableTeam team)
            {
                _teams.Remove(team);
                LoadStandings();
                LoadMostActivePlayers();
            }
        }
    }
}
