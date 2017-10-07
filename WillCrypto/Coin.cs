using System;
using System.Globalization;

namespace WillCrypto
{
    internal class Coin
    {
        public string MarketName { get; set; }
        public string BaseVolume { get; set; }
        public string Last { get; set; }
        public string OpenBuyOrders { get; set; }
        public string OpenSellOrders { get; set; }
        public string High { get; set; }
        public string Low { get; set; }

        public double GetVolume()
        {
            return double.Parse(BaseVolume, CultureInfo.InvariantCulture);
        }

        internal double GetLast()
        {
            return double.Parse(Last, CultureInfo.InvariantCulture);
        }

        internal string GetName()
        {
            return MarketName.ToLower().Split('-')[1];
        }
    }
}
