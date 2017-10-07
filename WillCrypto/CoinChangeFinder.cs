using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WillCrypto
{
    internal class CoinChangeFinder
    {
        private ExchangeAPI _bittrex;
        private List<Coin> _lastCoins;
        private ISocketMessageChannel _channel;
        private TrackedCoins _trackedCoins;

        internal CoinChangeFinder(ISocketMessageChannel channel)
        {
            _bittrex = new ExchangeAPI(@"https://bittrex.com/api/v1.1/public/");
            _lastCoins = new List<Coin>();
            _channel = channel;
            _trackedCoins = new TrackedCoins();
        }

        internal void Run(Object stateInfo)
        {
            CheckCoinsForVolumeChange();
        }

        internal void Stop()
        {
            _trackedCoins.StopTrackingAll();
        }


        private async Task CheckCoinsForVolumeChange()
        {
            List <Coin> coins = await _bittrex.GetAllCoins();
            foreach (Coin coin in coins)
            {
                Coin lastCoin = _lastCoins.Find(x => x.MarketName == coin.MarketName);
                if (lastCoin == null) continue;
                if (MeetsRequirements(coin, lastCoin)) _trackedCoins.Add(new TrackedCoin(coin, new CoinTracker(lastCoin, coin, _channel, _bittrex)));
            }
            _lastCoins = coins;
        }

        private bool MeetsRequirements(Coin coin, Coin lastCoin)
        {
            double volumeChange = Library.CalculateChange(lastCoin.GetVolume(), coin.GetVolume());
            double priceChange = Library.CalculateChange(lastCoin.GetLast(), coin.GetLast());
            return coin.MarketName.StartsWith("BTC") &&
                   priceChange > Settings.PriceTrackRequirement && 
                   volumeChange > Settings.VolumeTrackRequirement && 
                   !_trackedCoins.Contains(coin);
        }
    }
}
