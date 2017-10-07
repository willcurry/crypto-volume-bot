namespace WillCrypto
{
    internal class TrackedCoin
    {
        internal Coin Coin { get; private set; }
        internal CoinTracker CoinTracker { get; private set; }

        internal TrackedCoin(Coin coin, CoinTracker coinTracker)
        {
            Coin = coin;
            CoinTracker = coinTracker;
        }
    }
}
