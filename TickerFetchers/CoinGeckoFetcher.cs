using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

using System.Text.Json.Serialization;

namespace PortfolioTracker.TickerFetchers
{
    /// <summary>
    ///     Fetch crypto ticker information from CoinGecko.
    /// </summary>
    class CoinGeckoFetcher : ITickerFetcher
    {
        private const string API_URL = "https://api.coingecko.com/api/v3";

        // Remember Available Coins To Avoid Additional Requests
        private List<CoinGeckoListing> ListingCache;

        public CoinGeckoFetcher()
        {
            ListingCache = new List<CoinGeckoListing>();
        }

        public async Task<bool> Supports(string ticker)
        {
            return await GetListing(ticker) != null;
        }


        public async Task<MarketData> GetMarketData(string ticker)
        {
            CoinGeckoListing listing = await GetListing(ticker);
            if (listing == null)
            {
            //    return 0.0;
            }
            HttpClient httpClient = new HttpClient();
            string request = $"{API_URL}/coins/{listing.ID}?localization=false&tickers=false&market_data=true&community_data=false&developer_data=false&sparkline=false";
            JsonDocument response = JsonDocument.Parse(await httpClient.GetStringAsync(request));
            //return response.RootElement.GetProperty("market_data").GetProperty("current_price").GetProperty("usd").GetDouble();

            MarketData md = new MarketData("", 0, 0, 0, 0, 0);

            return md;
        }
        public async Task<double> GetPrice(string ticker)
        {
            CoinGeckoListing listing = await GetListing(ticker);
            if (listing == null)
            {
                return 0.0;
            }
            HttpClient httpClient = new HttpClient();
            string request = $"{API_URL}/coins/{listing.ID}?localization=false&tickers=false&market_data=true&community_data=false&developer_data=false&sparkline=false";
            JsonDocument response = JsonDocument.Parse(await httpClient.GetStringAsync(request));
            return response.RootElement.GetProperty("market_data").GetProperty("current_price").GetProperty("usd").GetDouble();
        }

        public async Task<string> GetName(string ticker)
        {
            CoinGeckoListing listing = await GetListing(ticker);
            if (listing == null)
            {
                return "Error";
            }
            return listing.Name;
        }

        private async Task<CoinGeckoListing> GetListing(string ticker)
        {
            // Check If Ticker Is Cached
            CoinGeckoListing listing = ListingCache.Find(item => item.Ticker.Equals(ticker, StringComparison.InvariantCultureIgnoreCase));
            // If Not, Update Listings & Check Again
            if (listing == null)
            {
                await UpdateListingsCache();
            }
            listing = ListingCache.Find(item => item.Ticker.Equals(ticker, StringComparison.InvariantCultureIgnoreCase));
            return listing;
        }

        private async Task UpdateListingsCache()
        {
            // Update Available CoinGecko Listings
            HttpClient httpClient = new HttpClient();
            string request = $"{API_URL}/coins/list?include_platform=false";
            string response = await httpClient.GetStringAsync(request);
            ListingCache = JsonSerializer.Deserialize<List<CoinGeckoListing>>(response);
        }
    }

    public class CoinGeckoListing
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }

        [JsonPropertyName("symbol")]
        public string Ticker { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
