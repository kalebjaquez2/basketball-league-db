using Backend.Models;
using Backend.Repositories;
using DataAccess;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Frontend
{
    /// <summary>
    /// Interaction logic for RosterPage.xaml
    /// </summary>
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}
