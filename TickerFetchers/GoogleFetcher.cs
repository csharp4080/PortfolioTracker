using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;


namespace PortfolioTracker.TickerFetchers
{
    class GoogleFetcher : ITickerFetcher
    {
        private const string URL = "https://www.google.com/finance/quote/";
        private readonly string[] exchanges = { "NASDAQ", "NYSE" };

        public async Task<bool> Supports(string ticker)
        {
            foreach (string exchange in exchanges)
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    string request = $"{URL}{ticker}:{exchange}";
                    var html = await httpClient.GetStringAsync(request);
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);
                    string price = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='YMlKec fxKbKc']").InnerText;
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public async Task<double> GetPrice(string ticker)
        {
            foreach (string exchange in exchanges)
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    string request = $"{URL}{ticker}:{exchange}";
                    var html = await httpClient.GetStringAsync(request);
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);
                    string price = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='YMlKec fxKbKc']").InnerText;
                    double dblprice = Convert.ToDouble(price);
                    return dblprice;
                }
                catch (Exception)
                {
                    return 0.0;
                }
            }
            return 0.0;
        }

        public async Task<string> GetName(string ticker)
        {
            foreach (string exchange in exchanges)
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    string request = $"{URL}{ticker}:{exchange}";
                    var html = await httpClient.GetStringAsync(request);
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);
                    string name = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='KY7mAb']").InnerText;
                    return name;
                }
                catch (Exception)
                {
                    return "Error";
                }
            }
            return "Error";
        }
    }
}
