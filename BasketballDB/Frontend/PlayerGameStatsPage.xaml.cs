using Backend.Models;
using Backend.Repositories;
using DataAccess;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Frontend
{
    public partial class BoxScorePage : Page
    {
        private readonly Game _game;
        private readonly string _connectionString;

        public BoxScorePage(Game game, string connectionString)
        {
            InitializeComponent();
            _game = game;
            _connectionString = connectionString;

            LoadBoxScore();
        }

        private void LoadBoxScore()
        {
            // Set the Team Names at the top
            HomeTeamHeader.Text = _game.HomeTeamName;
            AwayTeamHeader.Text = _game.AwayTeamName;
            HomeLabel.Text = $"{_game.HomeTeamName} ROSTER";
            AwayLabel.Text = $"{_game.AwayTeamName} ROSTER";

            var executor = new SqlCommandExecutor(_connectionString);
            var repo = new SqlPlayerGameStatsRepository(executor);

            // Fetch the stats
            // IMPORTANT: Ensure your Repository query joins the Players table 
            // so the 'PlayerName' property isn't null!
            var allStats = repo.RetrieveStatsByGame(_game.GameID);

            // 3. Filter and Bind
            HomeStatsGrid.ItemsSource = allStats.Where(s => s.TeamID == _game.HomeTeamID).ToList();
            AwayStatsGrid.ItemsSource = allStats.Where(s => s.TeamID == _game.AwayTeamID).ToList();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}