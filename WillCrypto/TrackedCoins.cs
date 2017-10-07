using System.Collections.Generic;

namespace WillCrypto
{
    internal class TrackedCoins
    {
        private List<TrackedCoin> _trackedCoins;

        internal TrackedCoins()
        {
            _trackedCoins = new List<TrackedCoin>();
        }

        internal void Add(TrackedCoin trackedCoin)
        {
            _trackedCoins.Add(trackedCoin);
        }

        internal bool Contains(Coin coin)
        {
            return _trackedCoins.Exists(trackedCoin => trackedCoin.Coin.MarketName == coin.MarketName);
        }

        internal void StopTrackingAll()
        {
            foreach (TrackedCoin trackedCoin in _trackedCoins)
                trackedCoin.CoinTracker.Stop();
        }

        internal void StopTracking(Coin coin)
        {
            foreach (TrackedCoin trackedCoin in _trackedCoins)
                if (trackedCoin.Coin.MarketName == coin.MarketName)
                    trackedCoin.CoinTracker.Stop();
        }
    }
}
