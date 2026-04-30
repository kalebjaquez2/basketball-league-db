using Backend.Models;
using Backend.Repositories;
using DataAccess;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Frontend
{
    public partial class PlayerGameStatsPage : Page
    {
        private readonly Game _game;
        private readonly string _connectionString;
        private DispatcherTimer? _debounceTimer;
        private EditablePlayerGameStats? _editingStats;

        public PlayerGameStatsPage(Game game, string connectionString)
        {
            InitializeComponent();
            _game = game;
            _connectionString = connectionString;
            LoadBoxScore();
            LoadTeamNames();
            LoadGameStatsSummary();

        }

        private void LoadBoxScore()
        {
            var executor = new SqlCommandExecutor(_connectionString);
            var repo = new SqlPlayerGameStatsRepository(executor);
            var allStats = repo.RetrieveStatsByGame(_game.GameID)
                .Select(s => new EditablePlayerGameStats(s))
                .ToList();

            HomeStatsGrid.ItemsSource = allStats.Where(s => s.TeamID == _game.HomeTeamID).ToList();
            AwayStatsGrid.ItemsSource = allStats.Where(s => s.TeamID == _game.AwayTeamID).ToList();
        }

        private void LoadTeamNames()
        {
            var executor = new SqlCommandExecutor(_connectionString);
            var repo = new SqlTeamRepository(executor);
            var homeTeam = repo.FetchTeam(_game.HomeTeamID);
            var awayTeam = repo.FetchTeam(_game.AwayTeamID);
            HomeTeamLabel.Text = homeTeam.TeamName;
            AwayTeamLabel.Text = awayTeam.TeamName;
            GameHeader.Text = $"{homeTeam.TeamName} vs {awayTeam.TeamName}";
        }

        private void LoadGameStatsSummary()
        {
            try
            {
                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlStatsRepository(executor);
                var gameDate = DateOnly.FromDateTime(_game.Date.ToDateTime(TimeOnly.MinValue));
                var summary = repo.RetrieveGameStatsSummary(gameDate, gameDate)
                    .FirstOrDefault(s => s.GameID == _game.GameID);

                if (summary != null)
                {
                    AvgPointsText.Text = summary.AveragePoints.ToString("F1");
                    AvgReboundsText.Text = summary.AverageRebounds.ToString("F1");
                    AvgAssistsText.Text = summary.AverageAssists.ToString("F1");
                    AvgTurnoversText.Text = summary.AverageTurnovers.ToString("F1");
                }
                else
                {
                    AvgPointsText.Text = "N/A";
                    AvgReboundsText.Text = "N/A";
                    AvgAssistsText.Text = "N/A";
                    AvgTurnoversText.Text = "N/A";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading game stats summary: " + ex.Message);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null && NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void StatsGrid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!Session.IsAdmin) return;
            if (sender is not DataGrid dg) return;

            var clicked = dg.SelectedItem as EditablePlayerGameStats;

            // Close any open row — if it's a different row, stop here so the
            // grid reloads cleanly before the user opens another one
            if (_editingStats != null && _editingStats != clicked)
            {
                CloseEditing(dg);
                return;
            }

            if (clicked == null)
            {
                CloseEditing(dg);
                return;
            }

            if (clicked.IsEditing)
                CloseEditing(dg);
            else
            {
                _editingStats = clicked;
                clicked.PropertyChanged += Stats_PropertyChanged;
                clicked.IsEditing = true;
                dg.SelectedItem = null;
            }
        }

        private void CloseEditing(DataGrid dg)
        {
            if (_editingStats == null) return;
            _debounceTimer?.Stop();
            _debounceTimer = null;
            _editingStats.PropertyChanged -= Stats_PropertyChanged;
            SaveStatsInternal(_editingStats);
            _editingStats.IsEditing = false;
            _editingStats = null;
            dg.SelectedItem = null;
            LoadBoxScore();
            LoadGameStatsSummary();
        }

        private void Stats_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is EditablePlayerGameStats stats && e.PropertyName?.StartsWith("Edit") == true)
                ScheduleSave(stats);
        }

        private void ScheduleSave(EditablePlayerGameStats stats)
        {
            _debounceTimer?.Stop();
            _debounceTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(600) };
            _debounceTimer.Tick += (s, e) =>
            {
                _debounceTimer!.Stop();
                SaveStatsInternal(stats);
            };
            _debounceTimer.Start();
        }

        private void SaveStatsInternal(EditablePlayerGameStats stats)
        {
            try
            {
                int minutes   = Parse(stats.EditMinutes);
                int rebounds  = Parse(stats.EditRebounds);
                int assists   = Parse(stats.EditAssists);
                int turnovers = Parse(stats.EditTurnovers);
                int steals    = Parse(stats.EditSteals);
                int blocks    = Parse(stats.EditBlocks);
                int fgMade    = Parse(stats.EditFGMade);
                int fgTaken   = Parse(stats.EditFGTaken);
                int threeMade = Parse(stats.EditThreeMade);
                int threeTaken= Parse(stats.EditThreeTaken);
                int fouls     = Parse(stats.EditFouls);

                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlPlayerGameStatsRepository(executor);
                repo.UpdatePlayerGameStats(
                    stats.PlayerID, stats.GameID, stats.TeamID,
                    minutes, turnovers, rebounds, assists, steals, blocks,
                    fgMade, fgTaken, threeMade, threeTaken, fouls
                );

                LoadGameStatsSummary();
            }
            catch (FormatException)
            {
                // User is mid-typing an invalid value — skip this save
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving stats: " + ex.Message);
            }
        }

        private static int Parse(string s) =>
            string.IsNullOrWhiteSpace(s) ? 0 : int.Parse(s);
    }
}
