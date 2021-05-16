using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker
{
    /// <summary>
    ///     An exception to be thrown when information for
    ///     an unsupported ticker is requested.
    /// </summary>
    public class TickerNotFoundException : Exception
    {
        public TickerNotFoundException()
        {
            // No Implementation Necessary
        }

        public TickerNotFoundException(string message) : base(message)
        {
            // No Implementation Necessary
        }

        public TickerNotFoundException(string message, Exception inner) : base(message, inner)
        {
            // No Implementation Necessary
        }
    }
}
