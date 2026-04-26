using System;
using System.Windows;
using Backend.Models;
using Backend.Repositories;
using DataAccess;

namespace Frontend
{
    public partial class AddTeamDialog : Window
    {
        private readonly int _seasonId;
        private readonly string _connectionString;

        public AddTeamDialog(int seasonId, string connectionString)
        {
            InitializeComponent();
            _seasonId = seasonId;
            _connectionString = connectionString;
            TeamNameBox.Focus();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TeamNameBox.Text))
            {
                ErrorMessage.Text = "Team name is required.";
                ErrorMessage.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlTeamRepository(executor);

                repo.CreateTeam(_seasonId, TeamNameBox.Text);

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = "Error: " + ex.Message;
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