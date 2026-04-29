using System;
using System.Linq;
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
            PageTitle.Text = $"{_team.TeamName.ToUpper()} SCHEDULE";
            LoadGames();
            if (!Session.IsAdmin)
                AddGameButton.Visibility = Visibility.Collapsed;
        }

        private void LoadGames()
        {
            try
            {
                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlGameRepository(executor);
                var games = repo.RetrieveGamesByTeam(_team.TeamID)
                    .Select(g => new EditableGame(g))
                    .ToList();
                GamesList.ItemsSource = games;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading games: " + ex.Message);
            }
        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddGameDialog(_seasonId, _connectionString);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
                LoadGames();
        }

        private void GameTile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is EditableGame eg)
                this.NavigationService.Navigate(new PlayerGameStatsPage(eg.Model, _connectionString));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void GameOptions_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                btn.ContextMenu.PlacementTarget = btn;
                btn.ContextMenu.IsOpen = true;
                e.Handled = true;
            }
        }

        private void EditGame_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi &&
                mi.Parent is ContextMenu cm &&
                cm.PlacementTarget is Button btn &&
                btn.Tag is EditableGame game)
            {
                game.IsEditing = true;
            }
        }

        private void SaveGame_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is EditableGame game)
            {
                if (!int.TryParse(game.EditHomeScore, out int homeScore) ||
                    !int.TryParse(game.EditAwayScore, out int awayScore))
                {
                    MessageBox.Show("Scores must be valid numbers.");
                    return;
                }

                try
                {
                    var executor = new SqlCommandExecutor(_connectionString);
                    var repo = new SqlGameRepository(executor);
                    repo.UpdateGame(game.GameID, homeScore, awayScore, game.OvertimeCount);
                    LoadGames();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving game: " + ex.Message);
                    game.IsEditing = false;
                }
            }
        }

        private void CancelGameEdit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is EditableGame game)
                game.IsEditing = false;
        }
    }
}
