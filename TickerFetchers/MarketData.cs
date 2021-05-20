using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.TickerFetchers
{
    public struct MarketData
    {
        public string name { get; set; }
        public double price { get; set; }
        public double marketcap { get; set; }
        public double dayrangehigh { get; set; }
        public double dayrangelow { get; set; }
        public double tradingvolume { get; set; }

        public MarketData(string n, double p, double m, double rh, double rl, double v)
        {
            name = n;
            price = p;
            marketcap = m;
            dayrangehigh = rh;
            dayrangelow = rl;
            tradingvolume = v;
        }

    }
}
