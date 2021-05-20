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

        public async Task<double> GetPrice(string ticker)
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

                    //Pull price as string
                    string price = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='YMlKec fxKbKc']").InnerText;

                    //Remove the $ from string
                    price = price.Remove(0, 1);

                    //Return price as double
                    return Convert.ToDouble(price);
                }

                else
                {
                    //Pull price as string
                    string price = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='YMlKec fxKbKc']").InnerText;

                    //Remove the $ from string
                    price = price.Remove(0, 1);

                    //Return price as double
                    return Convert.ToDouble(price);
                }
                    
            }
            catch (Exception) { }

            return 0.0;
        }

        public async Task<string> GetName(string ticker)
        {
            try
            {
                //Check NASDAQ first
                HttpClient httpClient = new HttpClient();
                string request = $"{URL}{ticker}:NASDAQ";
                var html = await httpClient.GetStringAsync(request);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var check = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='KY7mAb']");

                //If NASDAQ fails, check NYSE
                if (check == null)
                {
                    request = $"{URL}{ticker}:NYSE";
                    html = await httpClient.GetStringAsync(request);
                    htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);

                    string name = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='KY7mAb']").InnerText;

                    return name;
                }

                else
                {
                    string name = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='KY7mAb']").InnerText;

                    return name;
                }
            }
            catch (Exception) { }

            return "Error";
        }
    }
}
