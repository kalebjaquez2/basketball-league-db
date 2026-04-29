using Backend.Models;
using Backend.Repositories;
using DataAccess;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

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

            // Transparent button template — suppresses default WPF hover brightening
            // Must be declared here because navBtn (below) also uses it
            var transparentTemplate = new ControlTemplate(typeof(Button));
            var borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.BackgroundProperty, Brushes.Transparent);
            var cpFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            borderFactory.AppendChild(cpFactory);
            transparentTemplate.VisualTree = borderFactory;

            // Add Season button — admin only
            if (Session.IsAdmin)
            {
                var addStack = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
                addStack.Children.Add(new TextBlock
                {
                    Text = "+", FontSize = 40,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F05A28")),
                    HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness(0, 0, 0, 8)
                });
                addStack.Children.Add(new TextBlock
                {
                    Text = "Add Season", FontSize = 16,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999999")),
                    HorizontalAlignment = HorizontalAlignment.Center
                });
                var addBorder = new Border
                {
                    Width = 260, Height = 160, CornerRadius = new CornerRadius(8),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A1A")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#444444")),
                    BorderThickness = new Thickness(1), Child = addStack
                };
                var addButton = new Button
                {
                    Content = addBorder, Width = 260, Height = 160,
                    Margin = new Thickness(0, 0, 16, 16), Cursor = Cursors.Hand,
                    Background = Brushes.Transparent, BorderThickness = new Thickness(0)
                };
                addButton.Click += AddSeason_Click;
                addButton.Template = transparentTemplate;
                addButton.MouseEnter += (s, e) => addBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F05A28"));
                addButton.MouseLeave += (s, e) => addBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#444444"));
                TilesPanel.Children.Add(addButton);
            }

            foreach (var season in seasons)
            {
                // ── VIEW layer ────────────────────────────────
                var viewStack = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
                viewStack.Children.Add(new TextBlock
                {
                    Text = "🏀", FontSize = 32,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 12)
                });
                viewStack.Children.Add(new TextBlock
                {
                    Text = season.StartDate.Year.ToString(),
                    FontSize = 18, FontWeight = FontWeights.Bold, Foreground = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center
                });

                // ── EDIT layer (initially collapsed) ─────────
                var editStack = new StackPanel
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(14, 6, 14, 6),
                    Visibility = Visibility.Collapsed
                };

                var gray = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#999999"));

                editStack.Children.Add(new TextBlock
                {
                    Text = "START DATE", Foreground = gray,
                    FontSize = 10, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 2)
                });
                var startPicker = new DatePicker
                {
                    SelectedDate = season.StartDate.ToDateTime(TimeOnly.MinValue),
                    Height = 24, Margin = new Thickness(0, 0, 0, 4)
                };
                editStack.Children.Add(startPicker);

                editStack.Children.Add(new TextBlock
                {
                    Text = "END DATE", Foreground = gray,
                    FontSize = 10, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 0, 0, 2)
                });
                var endPicker = new DatePicker
                {
                    SelectedDate = season.EndDate.ToDateTime(TimeOnly.MinValue),
                    Height = 24, Margin = new Thickness(0, 0, 0, 6)
                };
                editStack.Children.Add(endPicker);

                var buttonRow = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
                var saveBtn = new Button
                {
                    Content = "✓  SAVE", Width = 88, Height = 26, Margin = new Thickness(0, 0, 6, 0),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E7D32")),
                    Foreground = Brushes.White, BorderThickness = new Thickness(0),
                    FontWeight = FontWeights.SemiBold, FontSize = 11, Cursor = Cursors.Hand
                };
                var cancelBtn = new Button
                {
                    Content = "✗", Width = 36, Height = 26,
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5A2A2A")),
                    Foreground = Brushes.White, BorderThickness = new Thickness(0),
                    FontWeight = FontWeights.SemiBold, FontSize = 13, Cursor = Cursors.Hand
                };
                buttonRow.Children.Add(saveBtn);
                buttonRow.Children.Add(cancelBtn);
                editStack.Children.Add(buttonRow);

                // ── Tile border (holds both layers) ──────────
                var tileContent = new Grid();
                tileContent.Children.Add(viewStack);
                tileContent.Children.Add(editStack);

                var tileBorder = new Border
                {
                    Width = 260, Height = 160, CornerRadius = new CornerRadius(8),
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1A1A1A")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E2E2E")),
                    BorderThickness = new Thickness(1), Child = tileContent, ClipToBounds = true
                };

                // ── Transparent nav overlay (on top of border, collapses in edit mode) ──
                var navBtn = new Button
                {
                    Width = 260, Height = 160,
                    Cursor = Cursors.Hand, Background = Brushes.Transparent,
                    BorderThickness = new Thickness(0), DataContext = season
                };
                navBtn.Click += SeasonTile_Click;
                navBtn.Template = transparentTemplate;

                // ── ⋮ options button ──────────────────────────
                var optionsBtn = new Button
                {
                    Content = "⋮", Width = 28, Height = 28,
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, 6, 6, 0),
                    Style = (Style)Application.Current.Resources["GhostButton"]
                };
                var editMenuItem = new MenuItem { Header = "Edit" };
                var contextMenu = new ContextMenu();
                contextMenu.Items.Add(editMenuItem);
                optionsBtn.ContextMenu = contextMenu;

                // Open context menu on ⋮ click
                optionsBtn.Click += (s, e) =>
                {
                    var ob = (Button)s;
                    ob.ContextMenu.PlacementTarget = ob;
                    ob.ContextMenu.IsOpen = true;
                    e.Handled = true;
                };

                // "Edit" → switch to edit mode
                editMenuItem.Click += (s, e) =>
                {
                    navBtn.Visibility = Visibility.Collapsed;
                    optionsBtn.Visibility = Visibility.Collapsed;
                    viewStack.Visibility = Visibility.Collapsed;
                    editStack.Visibility = Visibility.Visible;
                };

                // Save
                saveBtn.Click += (s, e) =>
                {
                    if (startPicker.SelectedDate == null || endPicker.SelectedDate == null)
                    {
                        MessageBox.Show("Please select both dates.");
                        return;
                    }
                    if (endPicker.SelectedDate <= startPicker.SelectedDate)
                    {
                        MessageBox.Show("End date must be after start date.");
                        return;
                    }
                    try
                    {
                        var exec = new SqlCommandExecutor(_connectionString);
                        new SqlSeasonRepository(exec).UpdateSeason(
                            season.SeasonID,
                            DateOnly.FromDateTime(startPicker.SelectedDate.Value),
                            DateOnly.FromDateTime(endPicker.SelectedDate.Value));
                        LoadSeasons();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                };

                // Cancel
                cancelBtn.Click += (s, e) =>
                {
                    startPicker.SelectedDate = season.StartDate.ToDateTime(TimeOnly.MinValue);
                    endPicker.SelectedDate = season.EndDate.ToDateTime(TimeOnly.MinValue);
                    editStack.Visibility = Visibility.Collapsed;
                    viewStack.Visibility = Visibility.Visible;
                    navBtn.Visibility = Visibility.Visible;
                    optionsBtn.Visibility = Visibility.Visible;
                };

                // Container: border on bottom, nav overlay on top, ⋮ on top-right
                var container = new Grid
                {
                    Width = 260, Height = 160,
                    Margin = new Thickness(0, 0, 16, 16)
                };
                container.Children.Add(tileBorder);
                container.Children.Add(navBtn);
                container.Children.Add(optionsBtn);
                container.MouseEnter += (s, e) => tileBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F05A28"));
                container.MouseLeave += (s, e) => tileBorder.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E2E2E"));
                TilesPanel.Children.Add(container);
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
                LoadSeasons();
        }

        private void SeasonTile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement el && el.DataContext is Season selectedSeason)
                this.NavigationService.Navigate(new TeamsPage(selectedSeason, _connectionString));
        }
    }
}
