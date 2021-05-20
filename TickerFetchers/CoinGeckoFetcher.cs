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
            MarketData marketData = new MarketData();
            // Get Unique API ID For Ticker
            CoinGeckoListing listing = await GetListing(ticker);
            if (listing == null)
            {
                return marketData;
            }
            // Request Coin Information
            HttpClient httpClient = new HttpClient();
            string request = $"{API_URL}/coins/{listing.ID}?localization=false&tickers=false&market_data=true&community_data=false&developer_data=false&sparkline=false";
            JsonDocument response = JsonDocument.Parse(await httpClient.GetStringAsync(request));
            JsonElement jsonMarketData = response.RootElement.GetProperty("market_data");
            // Extract Market Data From Response
            marketData.name = listing.Name;
            marketData.price = jsonMarketData.GetProperty("current_price").GetProperty("usd").GetDouble();
            marketData.marketcap = jsonMarketData.GetProperty("market_cap").GetProperty("usd").GetDouble();
            marketData.dayrangehigh = jsonMarketData.GetProperty("high_24h").GetProperty("usd").GetDouble();
            marketData.dayrangelow = jsonMarketData.GetProperty("low_24h").GetProperty("usd").GetDouble();
            marketData.tradingvolume = jsonMarketData.GetProperty("total_volume").GetProperty("usd").GetDouble();
            return marketData;
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
