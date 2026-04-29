
using Backend.Models;
using Backend.Repositories;
using DataAccess;
using System;
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
                var players = repo.RetrievePlayersByTeam(_team.TeamID);
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
                // Filter to only show players on this team
                var teamScorers = scorers
                    .Where(s => s.TeamID == _team.TeamID)
                    .ToList();
                TopScorersList.ItemsSource = teamScorers;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading top scorers: " + ex.Message);
            }
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
    }
}