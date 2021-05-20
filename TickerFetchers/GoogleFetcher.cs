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
                char mult = market[0][market[0].Length - 1];
                marketcap = market[0].Remove(market[0].Length - 1);

                double mc = Convert.ToDouble(marketcap);

                if (mult == 'M')
                    mc = mc * 1000000;

                if (mult == 'B')
                    mc = mc * 1000000000;

                mult = volume[volume.Length - 1];
                volume.Remove(volume.Length - 1);
                double vol = Convert.ToDouble(volume);

                if (mult == 'M')
                    vol = vol * 1000000;

                if (mult == 'B')
                    vol = vol * 1000000000;

                MarketData data = new MarketData(name, Convert.ToDouble(price), mc, Convert.ToDouble(range[2]), Convert.ToDouble(range[0]), vol);

                return data;

            }
            catch (Exception) { }

            MarketData md = new MarketData("", 0, 0, 0, 0, 0);
            return md;
        }
    }
}