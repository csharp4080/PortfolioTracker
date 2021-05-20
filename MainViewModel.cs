using PortfolioTracker.TickerFetchers;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PortfolioTracker
{
    public class MainViewModel
    {
        // Save File Information
        public string SaveFilePath { get; private set; }

        // Keep Track Of Selected Assets & Amount User Holds (If Applicable)
        public ObservableCollection<Ticker> TrackedTickers { get; private set; }

        // Ticker Data Sources
        private List<ITickerFetcher> StockFetchers;
        private List<ITickerFetcher> CryptoFetchers;

        /// <summary>
        ///     <para>
        ///         Create a view model based on existing user
        ///         preferences and profile data if found.
        ///     </para>
        ///     <para>
        ///         If no save file was found at the specified
        ///         location the view model will start with a new
        ///         initial state and save data to this file on exit.
        ///     </para>
        /// </summary>
        /// <param name="saveFilePath">
        ///     The path to the save file holding the data.
        /// </param>
        public MainViewModel(string saveFilePath)
        {
            // Initialize To Empty State
            SaveFilePath = saveFilePath;
            TrackedTickers = new ObservableCollection<Ticker>();
            // Attempt To Load Previously Saved State Information
            LoadDataFile();
            // Register Ticker Fetchers
            StockFetchers = new List<ITickerFetcher>
            {
                new GoogleFetcher()
            };
            CryptoFetchers = new List<ITickerFetcher>
            {
                new CoinGeckoFetcher()
            };
            // TODO: add more to support all relevant stocks / crypto
        }

        /// <summary>
        ///     Load state data from the file at SaveFilePath.
        /// </summary>
        private void LoadDataFile()
        {
            if (File.Exists(SaveFilePath))
            {
                StreamReader sr = File.OpenText(SaveFilePath);

                string s = sr.ReadLine();

                if(s != null)
                    TrackedTickers = JsonSerializer.Deserialize<ObservableCollection<Ticker>>(s);
            }

            // read file at SaveFilePath, load settings
            // ex. read tracked tickers/holdings from file, add into TrackedTickers mapping
        }

        /// <summary>
        ///     Save state data to the file at SaveFilePath.
        /// </summary>
        public void SaveDataFile()
        {
            string s = JsonSerializer.Serialize<ObservableCollection<Ticker>>(TrackedTickers);

            File.WriteAllText(SaveFilePath, s);

            // save settings into file at SaveFilePath to read at next startup
            // ex. for each tracked ticker/amount, save into file
        }

        private List<ITickerFetcher> GetSources(AssetType type)
        {
            return type switch
            {
                AssetType.STOCK => StockFetchers,
                AssetType.CRYPTO => CryptoFetchers,
                _ => throw new TickerNotFoundException($"Unsupported Type: {type}"),
            };
        }

        /// <summary>
        ///     Checks whether any of the current price fetching
        ///     sources supports the given ticker.
        /// </summary>
        /// <param name="ticker">
        ///     The ticker to check support for.
        /// </param>
        /// <returns>
        ///     True if a fetcher is capable of getting
        ///     information for this ticker.
        /// </returns>
        public async Task<bool> Supports(string ticker, AssetType type)
        {
            List<ITickerFetcher> sources = GetSources(type);
            // Check Each Fetcher Until One Supports The Ticker
            foreach (ITickerFetcher fetcher in sources)
            {
                if (await fetcher.Supports(ticker))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Check all available sources for the price of the
        ///     given ticker.
        /// </summary>
        /// <param name="ticker">
        ///     The name of the ticker to search the price of.
        /// </param>
        /// <returns>
        ///     A Task holding the price of the given ticker if
        ///     found, throws TickerNotFoundException if not found.
        /// </returns>
        public async Task<double> GetPrice(string ticker, AssetType type)
        {
            List<ITickerFetcher> sources = GetSources(type);
            // Check Each Fetcher Until One Supports The Ticker
            foreach (ITickerFetcher fetcher in sources)
            {
                if (await fetcher.Supports(ticker))
                {
                    return await fetcher.GetPrice(ticker);
                }
            }
            // Error If Ticker Not Found From Any Source
            throw new TickerNotFoundException($"Unsupported Ticker: {ticker}");
        }

        /// <summary>
        ///     Check all available sources for the price of the
        ///     given ticker.
        /// </summary>
        /// <param name="ticker">
        ///     The name of the ticker to search the price of.
        /// </param>
        /// <returns>
        ///     A Task holding the price of the given ticker if
        ///     found, throws TickerNotFoundException if not found.
        /// </returns>
        public async Task<string> GetName(string ticker, AssetType type)
        {
            List<ITickerFetcher> sources = GetSources(type);
            // Check Each Fetcher Until One Supports The Ticker
            foreach (ITickerFetcher fetcher in sources)
            {
                if (await fetcher.Supports(ticker))
                {
                    return await fetcher.GetName(ticker);
                }
            }
            // Error If Ticker Not Found From Any Source
            throw new TickerNotFoundException($"Unsupported Ticker: {ticker}");
        }
    }
}
