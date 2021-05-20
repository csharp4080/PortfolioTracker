using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using HtmlAgilityPack;


namespace PortfolioTracker.TickerFetchers
{    class GoogleFetcher : ITickerFetcher
    {
        private const string URL = "https://www.google.com/finance/quote/";
        //private readonly string[] exchanges = { "NASDAQ", "NYSE" };

        public async Task<bool> Supports(string ticker)
        {

            try
            {
                //Check NASDAQ first
                HttpClient httpClient = new HttpClient();
                string request = $"{URL}{ticker}:NASDAQ";
                var html = await httpClient.GetStringAsync(request);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var check = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='YMlKec fxKbKc']");

                //If NASDAQ fails, check NYSE
                if (check == null)
                {
                    request = $"{URL}{ticker}:NYSE";
                    html = await httpClient.GetStringAsync(request);
                    htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    check = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='YMlKec fxKbKc']");

                    //If both NASDAQ and NYSE fail, then the stock is invalid
                    if (check == null)
                        return false;

                    else
                        return true;
                }

                else
                    return true;
            }
            catch (Exception) { }

            return false;
        }

        public async Task<MarketData> GetMarketData(string ticker)
        {
            try
            {
                //Check NASDAQ first
                HttpClient httpClient = new HttpClient();
                string request = $"{URL}{ticker}:NASDAQ";
                var html = await httpClient.GetStringAsync(request);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var check = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='YMlKec fxKbKc']");

                //If NASDAQ fails, check NYSE
                if (check == null)
                {
                    request = $"{URL}{ticker}:NYSE";
                    html = await httpClient.GetStringAsync(request);
                    htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);
                }

                string name = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='KY7mAb']").InnerText;
                string price = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='YMlKec fxKbKc']").InnerText;

                var node = htmlDoc.DocumentNode.SelectSingleNode("//h2[@class'yV3rjd']");

                node = node.NextSibling;
                node = node.NextSibling;

                string dayrange = node.SelectSingleNode("//div[@class='P6K39c']").InnerText;

                node = node.NextSibling;
                node = node.NextSibling;

                string marketcap = node.SelectSingleNode("//div[@class='P6K39c']").InnerText;

                node = node.NextSibling;

                string volume = node.SelectSingleNode("//div[@class='P6K39c']").InnerText;


            }
            catch (Exception) { }

            MarketData md = new MarketData("", 0, 0, 0, 0, 0);
            return md;
        }
    }
}
