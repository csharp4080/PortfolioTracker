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
        public string Symbol { get; private set; }

        public AssetType Type { get; private set; }

        public double Price { get; set; }

        public double Ownership { get; set; }

        public Ticker(string symbol, AssetType type)
        {
            Symbol = symbol;
            Type = type;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Ticker))
            {
                return false;
            }
            Ticker other = obj as Ticker;
            return Symbol == other.Symbol && Type == other.Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Symbol, Type);
        }
    }
}
