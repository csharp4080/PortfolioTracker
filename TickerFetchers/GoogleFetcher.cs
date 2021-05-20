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

                HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='P6K39c']");

                string dayrange = nodes[6].InnerText;
                string marketcap = nodes[8].InnerText;
                string volume = nodes[9].InnerText;

                price = price.Remove(0, 1);

                string[] range = dayrange.Split(' ');
                range[0].Remove(0, 1);
                range[2].Remove(0, 1);

                string[] market = marketcap.Split(' ');
                char mult = market[0][market[0].Length];
                string marketprice = market[0].Remove(market[0].Length - 1);

                double mp = Convert.ToDouble(marketprice);

                if (mult == 'M')
                    mp = mp * 1000000;

                if (mult == 'B')
                    mp = mp * 1000000000;




                MarketData md = new MarketData(name, Convert.ToDouble(price), );

                return md;

            }
            catch (Exception) { }

            MarketData md = new MarketData("asdfasdf", 0, 0, 0, 0, 0);
            return md;
        }
    }
}
