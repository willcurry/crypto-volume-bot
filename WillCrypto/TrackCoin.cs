using Discord.WebSocket;
using System;

namespace WillCrypto
{
    internal class TrackCoin : Command
    {
        private ExchangeAPI _bittrex;
        private TrackedCoins _trackedCoins;

        internal TrackCoin()
        {
            _bittrex = new ExchangeAPI(@"https://bittrex.com/api/v1.1/public/");
            _trackedCoins = new TrackedCoins();
        }

        public bool AppliesTo(string message)
        {
            return message.StartsWith("track");
        }

        public MessageResponse Response(SocketMessage message)
        {
            Coin coin = _bittrex.GetCoinInfo(message.Content.Substring(6)).GetAwaiter().GetResult();
            if (coin != null)
            {
                if (_trackedCoins.Contains(coin))
                {
                    StopTracking(coin);
                    return new MessageResponse("Stopped tracking " + coin.MarketName, false, null);
                }
                StartTracking(coin, message.Channel);
                return new MessageResponse("Started tracking " + coin.MarketName, false, null);
            }
            return new MessageResponse("Coin could not be found.", false, null);
        }

        private void StartTracking(Coin coin, ISocketMessageChannel channel)
        {
            _trackedCoins.Add(new TrackedCoin(coin, new CoinTracker(coin, coin, channel, _bittrex)));
        }

        private void StopTracking(Coin coin)
        {
            _trackedCoins.StopTracking(coin);
        }
    }
}
