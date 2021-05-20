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

        int test = 0;
        private async void ButtonAddTicker_Click(object sender, RoutedEventArgs e)
        {
            string symbol = (test++ % 2 == 0) ? "BTC" : "F";
            Ticker ticker = new Ticker(symbol);
            if (!ViewModel.TrackedTickers.Contains(ticker) && await ViewModel.Supports(symbol))
            {
                ViewModel.TrackedTickers.Add(ticker);
            }
        }

        private async void lstTickers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ticker selected = lstTickers.Items[lstTickers.SelectedIndex] as Ticker;
            TickerSymbol.Content = selected.Symbol;
            TickerName.Content = await ViewModel.GetName(selected.Symbol);
            TickerPrice.Content = $"${await ViewModel.GetPrice(selected.Symbol):0.00}";
        }
    }
}
