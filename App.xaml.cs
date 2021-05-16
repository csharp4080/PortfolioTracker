using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PortfolioTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string SAVE_FILE_PATH = "portfolio.dat";

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Load Program State
            MainViewModel viewModel = new MainViewModel(SAVE_FILE_PATH);
            // Initialize GUI, Attach To State
            MainWindow mainWindow = new MainWindow();
            mainWindow.ViewModel = viewModel;
            // Show GUI
            mainWindow.Show();
        }
    }
}
