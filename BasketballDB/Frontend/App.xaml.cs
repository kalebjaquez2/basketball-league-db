using System.Configuration;
using System.Data;
using System.Windows;

namespace Frontend
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var login = new LoginWindow();
            if (login.ShowDialog() != true)
            {
                Shutdown();
                return;
            }

            ShutdownMode = ShutdownMode.OnLastWindowClose;
            var main = new MainWindow();
            main.Show();
        }
    }

}
