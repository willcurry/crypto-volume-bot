using Discord.WebSocket;
using System.Threading.Tasks;
using Discord;

namespace WillCrypto
{
    class FindCoin : Command
    {
        private ExchangeAPI _bittrex; 

        internal FindCoin()
        {
            _bittrex = new ExchangeAPI(@"https://bittrex.com/api/v1.1/public/");
        }

        public bool AppliesTo(string message)
        {
            return message.StartsWith("$");
        }

        public MessageResponse Response(SocketMessage message)
        {
            string coinToLookFor = message.Content.Substring(1);
            Coin coin = FindCoinAsync(coinToLookFor).GetAwaiter().GetResult();
            EmbedBuilder builder = new EmbedBuilder();
            builder.AddInlineField("Volume: ", coin.BaseVolume);
            builder.AddInlineField("Last Price: ", coin.Last);
            builder.AddInlineField("Buy Orders: ", coin.OpenBuyOrders);
            builder.AddInlineField("Sell Orders: ", coin.OpenSellOrders);
            builder.AddInlineField("24h High: ", coin.High);
            builder.AddInlineField("24h Low: ", coin.Low);
            builder.WithThumbnailUrl("https://www.investfeed.com/media/images/crypto-icons/" + coinToLookFor + ".png");

            builder.WithTitle(coin.MarketName);
            builder.WithColor(Color.Red);

            return new MessageResponse("", false, builder);
        }

        private async Task<Coin> FindCoinAsync(string coinToLookFor)
        {
            Coin coin = await _bittrex.GetCoinInfo(coinToLookFor);
            return coin;
        }
    }
}
