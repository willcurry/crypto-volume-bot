using Discord;
using Discord.WebSocket;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WillCrypto
{
    internal class CoinTracker
    {
        private Coin _initalCoin;
        private Coin _lastCoin;
        private Coin _updatedCoin;
        private ISocketMessageChannel _channel;
        private ExchangeAPI _bittrex;
        private Timer _timer;
        private int _pings;
        private int _strikes;

        internal CoinTracker(Coin intialCoin, Coin updatedCoin, ISocketMessageChannel channel, ExchangeAPI bittrex)
        {
            _initalCoin = intialCoin;
            _lastCoin = intialCoin;
            _updatedCoin = updatedCoin;
            _channel = channel;
            _bittrex = bittrex;
            _pings = 0;
            _strikes = 0;
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            _timer = new Timer(Track, autoEvent, 1000, 40000);
        }

        internal void Stop()
        {
            _timer.Dispose();
        }

        private void NotifyChannel()
        {
            string overallVolumeChange = Library.DoubleToPercentageString(Library.CalculateChange(_initalCoin.GetVolume(), _updatedCoin.GetVolume()));
            string overallPriceChange = Library.DoubleToPercentageString(Library.CalculateChange(_initalCoin.GetLast(), _updatedCoin.GetLast()));
            EmbedBuilder builder = new EmbedBuilder();
            builder.AddInlineField("Volume (initial): ", _initalCoin.BaseVolume);
            builder.AddInlineField("Volume (now): ", _updatedCoin.BaseVolume);
            builder.AddInlineField("Price (initial): ", _initalCoin.Last);
            builder.AddInlineField("Price (now): ", _updatedCoin.Last);
            builder.AddInlineField("Volume Change: ", overallVolumeChange);
            builder.AddInlineField("Price Change: ", overallPriceChange);
            builder.WithThumbnailUrl("https://www.investfeed.com/media/images/crypto-icons/" + _initalCoin.GetName() + ".png");
            builder.WithTitle(_initalCoin.MarketName);
            builder.WithFooter("Pings: " + _pings + " | Strikes: " + _strikes);
            builder.WithColor(Color.DarkPurple);
            _channel.SendMessageAsync("", false, builder);
        }

        private void Track(Object stateInfo)
        {
            if (ShouldStop())
            {
                Stop();
                return;
            }
            _pings++;
            _lastCoin = _updatedCoin;
            NotifyChannel();
            UpdateCoin();
        }

        private bool ShouldStop()
        {
            if (FailsRequirements()) _strikes++;
            return _pings > 2 && _strikes > 3;
        }

        private bool FailsRequirements()
        {
            double lastVolumeChange = Library.CalculateChange(_lastCoin.GetVolume(), _updatedCoin.GetVolume());
            double lastPriceChange = Library.CalculateChange(_lastCoin.GetLast(), _updatedCoin.GetLast());
            return lastPriceChange < Settings.PriceStrikeRequirement || lastVolumeChange < Settings.VolumeStrikeRequirement;
        }

        private async Task UpdateCoin()
        {
            _updatedCoin = await _bittrex.GetCoinInfo(_initalCoin.MarketName);
        }
    }
}
