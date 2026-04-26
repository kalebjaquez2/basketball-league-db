using System;
using System.Windows;
using Backend.Models;
using Backend.Repositories;
using DataAccess;

namespace Frontend
{
    public partial class AddSeasonDialog : Window
    {
        private readonly League _league;
        private readonly string _connectionString;

        public AddSeasonDialog(League league, string connectionString)
        {
            InitializeComponent();
            _league = league;
            _connectionString = connectionString;

            // Default dates to today and 3 months from now
            StartDatePicker.SelectedDate = DateTime.Today;
            EndDatePicker.SelectedDate = DateTime.Today.AddMonths(3);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
                {
                    ShowError("Please select both dates.");
                    return;
                }

                if (EndDatePicker.SelectedDate <= StartDatePicker.SelectedDate)
                {
                    ShowError("End date must be after start date.");
                    return;
                }

                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlSeasonRepository(executor);

                // Update your repo call to use DateOnly.FromDateTime()
                repo.CreateSeason(
                    _league.LeagueID,
                    DateOnly.FromDateTime(StartDatePicker.SelectedDate.Value),
                    DateOnly.FromDateTime(EndDatePicker.SelectedDate.Value)
                );

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                ShowError("Database error: " + ex.Message);
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
