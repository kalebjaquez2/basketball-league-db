
using Backend.Models;
using Backend.Repositories;
using DataAccess;
using System;
using System.Collections.ObjectModel;
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
        private ObservableCollection<EditablePlayer> _players = new();

        public RosterPage(Team team, string connectionString)
        {
            InitializeComponent();
            _team = team;
            _connectionString = connectionString;
            PageTitle.Text = $"{_team.TeamName.ToUpper()} ROSTER";
            LoadPlayers();
            LoadTopScorers();
            if (!Session.IsAdmin)
                AddPlayerButton.Visibility = Visibility.Collapsed;
            else
                GenerateRosterButton.Visibility = Visibility.Visible;
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
                _players = new ObservableCollection<EditablePlayer>(players);
                PlayersList.ItemsSource = _players;
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

        private void GenerateRoster_Click(object sender, RoutedEventArgs e)
        {
            var rng = new Random();
            string[] firstNames = ["Marcus", "Derek", "Tyrone", "Calvin", "Jerome",
                                   "Andre", "Lamar", "Darnell", "Kenny", "Reggie",
                                   "Terrence", "Malik", "Darius", "Victor", "Byron",
                                   "Cedric", "Nathan", "Elijah", "Oscar", "Willis"];
            string[] lastNames  = ["Webb", "Hollins", "Bass", "Morrow", "Patel",
                                   "Simmons", "Hughes", "Cruz", "Foster", "Banks",
                                   "Price", "Stone", "Cole", "James", "King",
                                   "Ross", "Perry", "Reed", "Rivera", "Morgan"];
            string[] heights    = ["5'10\"", "6'0\"", "6'1\"", "6'2\"",
                                   "6'3\"", "6'4\"", "6'5\"", "6'6\""];
            (string pos, int minHt, int maxWt)[] slots =
            [
                ("PG", 2, 3),   // index into heights for typical range
                ("SG", 3, 4),
                ("SF", 4, 5),
                ("PF", 5, 6),
                ("C",  6, 7),
            ];

            var usedJerseys = _players.Select(p => p.JerseyNumber).ToHashSet();
            var executor = new SqlCommandExecutor(_connectionString);
            var repo = new SqlPlayerRepository(executor);

            foreach (var (pos, htMin, htMax) in slots)
            {
                int jersey = rng.Next(1, 100);
                while (usedJerseys.Contains(jersey))
                    jersey = rng.Next(1, 100);
                usedJerseys.Add(jersey);

                repo.CreatePlayer(
                    _team.TeamID,
                    jersey,
                    firstNames[rng.Next(firstNames.Length)],
                    lastNames[rng.Next(lastNames.Length)],
                    pos,
                    rng.Next(20, 36),
                    heights[rng.Next(htMin, htMax + 1)],
                    rng.Next(170, 240)
                );
            }

            LoadPlayers();
            LoadTopScorers();
        }

        private void AddPlayer_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddPlayerDialog(_team.TeamID, _connectionString);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                LoadPlayers();
                LoadTopScorers();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void PlayerOptions_Click(object sender, RoutedEventArgs e)
        {
            if (!Session.IsAdmin) return;
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

                    int? age = int.TryParse(player.EditAge, out int parsedAge)
                        ? parsedAge : null;

                    string? height = string.IsNullOrWhiteSpace(player.EditHeight)
                        ? null : player.EditHeight.Trim();

                    int? weight = int.TryParse(player.EditWeight, out int parsedWeight)
                        ? parsedWeight : null;

                    var executor = new SqlCommandExecutor(_connectionString);
                    var repo = new SqlPlayerRepository(executor);
                    repo.UpdatePlayer(player.PlayerID, jerseyNum, pos, age, height, weight);
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

        private void DeletePlayer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem mi &&
                mi.Parent is ContextMenu cm &&
                cm.PlacementTarget is Button btn &&
                btn.Tag is EditablePlayer player)
            {
                _players.Remove(player);
            }
        }
    }
}
