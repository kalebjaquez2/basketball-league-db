using Backend.Models;
using Backend.Repositories;
using DataAccess;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Frontend
{
    public partial class SeasonsPage : Page
    {
        private readonly League _league;
        private readonly string _connectionString;

        public SeasonsPage(League league, string connectionString)
        {
            InitializeComponent();
            _league = league;
            _connectionString = connectionString;
            LeagueHeader.Text = league.LeagueName;
            LoadSeasons();
        }

        private void LoadSeasons()
        {
            var executor = new SqlCommandExecutor(_connectionString);
            var repo = new SqlSeasonRepository(executor);
            var seasons = repo.RetrieveSeasonsByLeague(_league.LeagueID);

            TilesPanel.Children.Clear();

            // Add Season button
            var addStack = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
            addStack.Children.Add(new TextBlock
            {
                Text = "+",
                FontSize = 40,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F05A28")),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 8)
            });
            addStack.Children.Add(new TextBlock
            {
                Text = "Add Season",
                FontSize = 16,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999999")),
                HorizontalAlignment = HorizontalAlignment.Center
            });

            var addBorder = new Border
            {
                Width = 260,
                Height = 160,
                CornerRadius = new CornerRadius(8),
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A1A")),
                BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#444444")),
                BorderThickness = new Thickness(1),
                Child = addStack
            };

            var addButton = new Button
            {
                Content = addBorder,
                Width = 260,
                Height = 160,
                Margin = new Thickness(0, 0, 16, 16),
                Cursor = Cursors.Hand,
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0)
            };
            addButton.Click += AddSeason_Click;
            TilesPanel.Children.Add(addButton);

            foreach (var season in seasons)
            {
                var stack = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
                stack.Children.Add(new TextBlock
                {
                    Text = "🏀",
                    FontSize = 32,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 12)
                });
                stack.Children.Add(new TextBlock
                {
                    Text = season.StartDate.Year.ToString(),
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    Foreground = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                var border = new Border
                {
                    Width = 260,
                    Height = 160,
                    CornerRadius = new CornerRadius(8),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A1A")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E2E2E")),
                    BorderThickness = new Thickness(1),
                    Child = stack
                };

                var btn = new Button
                {
                    Content = border,
                    Width = 260,
                    Height = 160,
                    Margin = new Thickness(0, 0, 16, 16),
                    Cursor = Cursors.Hand,
                    Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0),
                    DataContext = season
                };
                btn.Click += SeasonTile_Click;
                TilesPanel.Children.Add(btn);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) is MainWindow mainWindow)
            {
                mainWindow.MainFrame.Visibility = Visibility.Collapsed;
                mainWindow.HomeScreen.Visibility = Visibility.Visible;
            }
        }

        private void AddSeason_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddSeasonDialog(_league, _connectionString);
            dialog.Owner = Window.GetWindow(this);
            if (dialog.ShowDialog() == true)
            {
                LoadSeasons();
            }
        }

        private void SeasonTile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Season selectedSeason)
            {
                this.NavigationService.Navigate(new TeamsPage(selectedSeason, _connectionString));
            }
        }
    }
}