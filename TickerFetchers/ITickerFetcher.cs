using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker
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
        ///     Retrieve the current price of the asset
        ///     specified by the ticker.
        /// </summary>
        /// <param name="ticker">
        ///     The ticker to check the price of.
        /// </param>
        /// <returns>
        ///     A Task that will hold the current price
        ///     if the lookup was successful.
        /// </returns>
        Task<double> GetPrice(string ticker);

        /// <summary>
        ///     Retrieve the full name of the asset or company
        ///     represented by the ticker.
        /// </summary>
        /// <param name="ticker">
        ///     The ticker symbol.
        /// </param>
        /// <returns>
        ///     The full name of the asset or company.
        /// </returns>
        Task<string> GetName(string ticker);
    }
}