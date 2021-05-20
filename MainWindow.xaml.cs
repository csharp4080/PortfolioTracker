using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PortfolioTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel {private set; get;}
        public MainWindow(MainViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void ButtonAddTicker_Click(object sender, RoutedEventArgs e)
        {
            // Disable Add Button
            ButtonAddTicker.IsEnabled = false;
            // Close Market View
            MarketData.IsEnabled = false;
            MarketData.Visibility = Visibility.Hidden;
            // Open New Ticker View
            AddTicker.Visibility = Visibility.Visible;
            AddTicker.IsEnabled = true;
        }

        private void ButtonCancelNewTicker_Click(object sender, RoutedEventArgs e)
        {
            ExitNewTickerView();
        }

        private async void ButtonSaveNewTicker_Click(object sender, RoutedEventArgs e)
        {
            Ticker ticker = new Ticker(TextBoxNewTickerSymbol.Text);
            ticker.Ownership = Double.Parse(TextBoxNewTickerShare.Text);
            if (!ViewModel.TrackedTickers.Contains(ticker) && await ViewModel.Supports(ticker.Symbol))
            {
                ViewModel.TrackedTickers.Add(ticker);
                ExitNewTickerView();
            }
        }

        private void ExitNewTickerView()
        {
            // Close New Ticker View
            AddTicker.IsEnabled = false;
            AddTicker.Visibility = Visibility.Hidden;
            // Open Market View
            MarketData.Visibility = Visibility.Visible;
            MarketData.IsEnabled = true;
            // Re-enable Add Button
            ButtonAddTicker.IsEnabled = true;
        }

        private async void lstTickers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ticker selected = lstTickers.Items[lstTickers.SelectedIndex] as Ticker;
            TickerSymbol.Content = selected.Symbol;
            TickerName.Content = "";
            TickerPrice.Content = "";
            TickerName.Content = await ViewModel.GetName(selected.Symbol);
            TickerPrice.Content = $"${await ViewModel.GetPrice(selected.Symbol):0.00}";
        }

        private void PortfolioTracker_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.SaveDataFile();
        }
    }
}
