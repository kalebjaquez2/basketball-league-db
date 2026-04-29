
using Backend.Models;
using Backend.Repositories;
using DataAccess;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Frontend
{
    public partial class RosterPage : Page
    {
        private readonly Team _team;
        private readonly string _connectionString;

        public RosterPage(Team team, string connectionString)
        {
            InitializeComponent();
            _team = team;
            _connectionString = connectionString;
            PageTitle.Text = $"{_team.TeamName.ToUpper()} ROSTER";
            LoadPlayers();
            LoadTopScorers();
        }

        private void LoadPlayers()
        {
            try
            {
                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlPlayerRepository(executor);
                var players = repo.RetrievePlayersByTeam(_team.TeamID)
                    .Select(p => new EditablePlayer(p))
                    .ToList();
                PlayersList.ItemsSource = players;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading roster: " + ex.Message);
            }
        }

        private void LoadTopScorers()
        {
            try
            {
                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlStatsRepository(executor);
                var scorers = repo.RetrieveTopScorersByTeam(_team.SeasonID);
                TopScorersList.ItemsSource = scorers
                    .Where(s => s.TeamID == _team.TeamID)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading top scorers: " + ex.Message);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void PlayerOptions_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.ContextMenu.PlacementTarget = btn;
                btn.ContextMenu.IsOpen = true;
                e.Handled = true;
            }
        }

        private void EditPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi &&
                mi.Parent is ContextMenu cm &&
                cm.PlacementTarget is Button btn &&
                btn.Tag is EditablePlayer player)
            {
                player.IsEditing = true;
            }
        }

        private void SavePlayer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is EditablePlayer player)
            {
                if (!int.TryParse(player.EditJerseyNumber, out int jerseyNum))
                {
                    MessageBox.Show("Jersey number must be a valid number.");
                    return;
                }

                try
                {
                    string? pos = string.IsNullOrWhiteSpace(player.EditPosition)
                        ? null : player.EditPosition.Trim();

                    var executor = new SqlCommandExecutor(_connectionString);
                    var repo = new SqlPlayerRepository(executor);
                    repo.UpdatePlayer(player.PlayerID, jerseyNum, pos,
                        player.Model.Age, player.Model.Height, player.Model.Weight);
                    LoadPlayers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving player: " + ex.Message);
                    player.IsEditing = false;
                }
            }
        }

        private void CancelPlayerEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is EditablePlayer player)
                player.IsEditing = false;
        }
    }
}
