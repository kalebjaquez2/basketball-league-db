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
            try
            {
                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlTeamRepository(executor);
                var teams = repo.RetrieveTeamsBySeason(_season.SeasonID);

                TeamsList.ItemsSource = teams;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading teams: " + ex.Message);
            }
        }

        private void AddTeam_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddTeamDialog(_season.SeasonID, _connectionString);
            dialog.Owner = Window.GetWindow(this);

            if (dialog.ShowDialog() == true)
            {
                LoadTeams(); // Refresh the list after adding
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