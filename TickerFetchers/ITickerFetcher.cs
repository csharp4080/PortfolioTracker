using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.TickerFetchers
{
    //  <summary>
    //      An interface to allow for asynchronous ticker
    //      information requests from multiple sources.
    //  </summary>
    interface ITickerFetcher
    {
        /// <summary>
        ///     Whether or not this information fetcher
        ///     supports the given ticker.
        /// </summary>
        /// <param name="ticker">
        ///     The ticker to check support for.
        /// </param>
        /// <returns>
        ///     A Task that will hold a true result if
        ///     this fetcher can lookup data for the ticker.
        /// </returns>
        Task<bool> Supports(string ticker);
        
        /// <summary>
        ///     Retrieve all the market data of the asset
        ///     specified by the ticker.
        /// </summary>
        /// <param name="ticker">
        ///     The ticker to check the data of.
        /// </param>
        /// <returns>
        ///     A Task that will hold the current info
        ///     if the lookup was successful.
        /// </returns>
        Task<MarketData> GetMarketData(string ticker);
    }
}