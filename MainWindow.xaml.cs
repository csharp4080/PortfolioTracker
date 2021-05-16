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
        public MainViewModel ViewModel {set; get;}
        public MainWindow()
        {
            InitializeComponent();
        }

        int test = 0;
        private async void ButtonAddTicker_Click(object sender, RoutedEventArgs e)
        {
            string ticker = (test++ % 2 == 0) ? "BTC" : "VTC";
            if (await ViewModel.Supports(ticker))
            {
                lstTickers.Items.Add(ticker);
            }
        }

        private async void lstTickers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selected = (string) lstTickers.Items[lstTickers.SelectedIndex];
            TickerSymbol.Content = selected;
            TickerName.Content = await ViewModel.GetName(selected);
            TickerPrice.Content = $"${await ViewModel.GetPrice(selected):0.00}";
        }
    }
}
