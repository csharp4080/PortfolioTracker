using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.TickerFetchers
{
    public struct MarketData
    {
        public string name;
        public double price;
        public double marketcap;
        public double dayrangehigh;
        public double dayrangelow;
        public double tradingvolume;

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
