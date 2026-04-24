using Backend.Repositories;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Frontend
{
    public partial class AddLeagueDialog : Window
    {
        public string? NewLeagueName { get; private set; }
        public string? NewLocation { get; private set; }

        private readonly ILeagueRepository _leagueRepository;

        public AddLeagueDialog(ILeagueRepository leagueRepository)
        {
            InitializeComponent();
            _leagueRepository = leagueRepository;
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SaveButton == null) return;
            SaveButton.IsEnabled = !string.IsNullOrWhiteSpace(LeagueNameBox.Text)
                                && !string.IsNullOrWhiteSpace(LocationBox.Text);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = LeagueNameBox.Text.Trim();

            // Using 1 as a placeholder for the LocationID until you add a dropdown
            int locationId = 1;

            try
            {
                // This calls the code we just fixed above
                var result = _leagueRepository.CreateLeague(name, locationId);

                if (result != null)
                {
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Database Error: {ex.Message}");
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }
    }
}
