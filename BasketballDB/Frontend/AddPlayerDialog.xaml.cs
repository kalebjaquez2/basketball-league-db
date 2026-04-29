using System;
using System.Windows;
using System.Windows.Controls;
using Backend.Repositories;
using DataAccess;

namespace Frontend
{
    public partial class AddPlayerDialog : Window
    {
        private readonly int _teamId;
        private readonly string _connectionString;

        public AddPlayerDialog(int teamId, string connectionString)
        {
            InitializeComponent();
            _teamId = teamId;
            _connectionString = connectionString;
            FirstNameBox.Focus();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Visibility = Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(FirstNameBox.Text))
            {
                ShowError("First name is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(LastNameBox.Text))
            {
                ShowError("Last name is required.");
                return;
            }

            if (!int.TryParse(JerseyNumberBox.Text.Trim(), out int jerseyNumber))
            {
                ShowError("Jersey number must be a whole number.");
                return;
            }

            if (PositionBox.SelectedItem is not ComboBoxItem posItem)
            {
                ShowError("Position is required.");
                return;
            }
            string position = posItem.Content.ToString()!;

            int? age = null;
            if (!string.IsNullOrWhiteSpace(AgeBox.Text))
            {
                if (!int.TryParse(AgeBox.Text.Trim(), out int parsedAge))
                {
                    ShowError("Age must be a whole number.");
                    return;
                }
                age = parsedAge;
            }

            string? height = string.IsNullOrWhiteSpace(HeightBox.Text)
                ? null
                : HeightBox.Text.Trim();

            int? weight = null;
            if (!string.IsNullOrWhiteSpace(WeightBox.Text))
            {
                if (!int.TryParse(WeightBox.Text.Trim(), out int parsedWeight))
                {
                    ShowError("Weight must be a whole number.");
                    return;
                }
                weight = parsedWeight;
            }

            try
            {
                var executor = new SqlCommandExecutor(_connectionString);
                var repo = new SqlPlayerRepository(executor);
                repo.CreatePlayer(_teamId, jerseyNumber,
                    FirstNameBox.Text.Trim(), LastNameBox.Text.Trim(),
                    position, age, height, weight);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                ShowError("Error: " + ex.Message);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ShowError(string message)
        {
            ErrorMessage.Text = message;
            ErrorMessage.Visibility = Visibility.Visible;
        }
    }
}
