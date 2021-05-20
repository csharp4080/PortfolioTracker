using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker
{
    /// <summary>
    ///     A class to keep track of information for a ticker.
    /// </summary>
    public class Ticker
    {
        public String Symbol { get; set; }

        public double Price { get; set; }

        public double Ownership { get; set; }

        public Ticker(String symbol)
        {
            Symbol = symbol;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Ticker))
            {
                return false;
            }
            Ticker other = obj as Ticker;
            return Symbol == other.Symbol;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Symbol);
        }
    }
}
