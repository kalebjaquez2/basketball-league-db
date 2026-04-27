using System;
using System.Collections.Generic;
using System.Windows;
using Backend.Models;
using Backend.Repositories;
using DataAccess;

namespace Frontend
{
    public partial class AddGameDialog : Window
    {
        private readonly int _seasonId;
        private readonly string _connectionString;

        public AddGameDialog(int seasonId, string connectionString)
        {
            InitializeComponent();
            _seasonId = seasonId;
            _connectionString = connectionString;
            LoadTeams();
            GameDatePicker.SelectedDate = DateTime.Today;
        }

        private void LoadTeams()
        {
            var executor = new SqlCommandExecutor(_connectionString);
            var repo = new SqlTeamRepository(executor);
            var teams = repo.RetrieveTeamsBySeason(_seasonId);

            HomeTeamCombo.ItemsSource = teams;
            AwayTeamCombo.ItemsSource = teams;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate Team Selection
                if (HomeTeamCombo.SelectedItem == null || AwayTeamCombo.SelectedItem == null)
                    throw new Exception("Please select both teams.");

                var homeTeam = (Team)HomeTeamCombo.SelectedItem;
                var awayTeam = (Team)AwayTeamCombo.SelectedItem;

                if (homeTeam.TeamID == awayTeam.TeamID)
                    throw new Exception("A team cannot play against itself.");

                // Validate Date 
                if (!GameDatePicker.SelectedDate.HasValue)
                    throw new Exception("Please select a game date.");

                // Validate Court
                if (!int.TryParse(CourtBox.Text, out int court))
                    throw new Exception("Invalid court number.");

                // Execute Data Access
                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlGameRepository(executor);

                repo.CreateGame(
                    homeTeam.TeamID,
                    awayTeam.TeamID,
                    homeTeam.TeamName, 
                    awayTeam.TeamName,
                    0,                 // homeTeamScore
                    0,                 // awayTeamScore
                    court,
                    0,                 // overtimeCount
                    DateOnly.FromDateTime(GameDatePicker.SelectedDate.Value)
                );

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = ex.Message;
                ErrorMessage.Visibility = Visibility.Visible;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}