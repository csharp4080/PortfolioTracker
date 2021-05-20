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
using PortfolioTracker.TickerFetchers;

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
            EnterNewTickerView();
        }

        private void ButtonCancelNewTicker_Click(object sender, RoutedEventArgs e)
        {
            ExitNewTickerView();
        }

        private async void ButtonSaveNewTicker_Click(object sender, RoutedEventArgs e)
        {
            AssetType type;
            if ((bool) RadioOptionStock.IsChecked)
            {
                type = AssetType.STOCK;
            }
            else
            {
                type = AssetType.CRYPTO;
            }
            Ticker ticker = new Ticker(TextBoxNewTickerSymbol.Text, type);
            ticker.Ownership = Double.Parse(TextBoxNewTickerShare.Text);
            if (ViewModel.TrackedTickers.Contains(ticker))
            {
                ticker = ViewModel.TrackedTickers.Where(t => t.Equals(ticker)).First();
                ticker.Ownership = Double.Parse(TextBoxNewTickerShare.Text);
                ExitNewTickerView();
            }
            else if (await ViewModel.Supports(ticker.Symbol, ticker.Type))
            {
                ViewModel.TrackedTickers.Add(ticker);
                ExitNewTickerView();
            }
            else
            {
                TextBoxNewTickerSymbol.BorderBrush = Brushes.DarkRed;
                TextBoxNewTickerSymbol.Foreground = Brushes.Red;
            }
        }

        private void EnterNewTickerView()
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

        private void ExitNewTickerView()
        {
            // Disable & Reset Controls
            AddTicker.IsEnabled = false;
            TextBoxNewTickerSymbol.Text = "";
            TextBoxNewTickerShare.Text = "";
            // Close New Ticker View
            AddTicker.Visibility = Visibility.Hidden;
            // Open Market View
            MarketData.Visibility = Visibility.Visible;
            MarketData.IsEnabled = true;
            // Re-enable Add Button
            ButtonAddTicker.IsEnabled = true;
        }

        private async void lstTickers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear Old Values
            TickerSymbol.Content = "";
            TickerName.Content = "";
            TickerPrice.Content = "";
            YourPortfolioAmount.Content = "";
            MarketCap.Content = "";
            Volume.Content = "";
            DayLow.Content = "";
            DayHigh.Content = "";
            // If None Selected Leave Blank
            if (lstTickers.SelectedIndex < 0)
            {
                return;
            }
            // Otherwise Get New Info
            Ticker ticker = lstTickers.Items[lstTickers.SelectedIndex] as Ticker;
            TickerSymbol.Content = ticker.Symbol;
            YourPortfolioAmount.Content = ticker.Ownership;
            MarketData tickerData = await ViewModel.GetMarketData(ticker.Symbol, ticker.Type);
            TickerName.Content = tickerData.name;
            TickerPrice.Content = $"${tickerData.price:0.00}";
            ShareValue.Content = $"${tickerData.price * ticker.Ownership:0.00}";
            MarketCap.Content = $"${tickerData.marketcap:0.00}";
            Volume.Content = $"${tickerData.tradingvolume:0.00}";
            DayLow.Content = $"${tickerData.dayrangelow:0.00}";
            DayHigh.Content = $"${tickerData.dayrangehigh:0.00}";
        }

        private void PortfolioTracker_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.SaveDataFile();
        }

        private void ButtonRemoveTicker_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int index = lstTickers.Items.IndexOf(button.DataContext);
            ViewModel.TrackedTickers.RemoveAt(index);
        }

        private void ButtonModifyTicker_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Ticker ticker = ViewModel.TrackedTickers[lstTickers.Items.IndexOf(button.DataContext)];
            TextBoxNewTickerSymbol.Text = ticker.Symbol;
            if (ticker.Type == AssetType.STOCK)
            {
                RadioOptionStock.IsChecked = true;
            }
            else
            {
                RadioOptionCrypto.IsChecked = true;
            }
            EnterNewTickerView();
        }

        private void TextBoxNewTickerSymbol_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBoxNewTickerSymbol.BorderBrush = Brushes.Black;
            TextBoxNewTickerSymbol.Foreground = Brushes.Black;
            TextBox textBox = sender as TextBox;
            Ticker t = new Ticker(textBox.Text, (bool) RadioOptionStock.IsChecked ? AssetType.STOCK : AssetType.CRYPTO);
            Ticker ticker = ViewModel.TrackedTickers.Where(ticker => ticker.Equals(t)).FirstOrDefault();
            if (ticker == null)
            {
                return;
            }
            TextBoxNewTickerShare.Text = "" + ticker.Ownership;
        }
    }
}