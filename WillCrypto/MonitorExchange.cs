using Discord.WebSocket;
using System.Threading;

namespace WillCrypto
{
    internal class MonitorExchange : Command
    {
        private Timer _timer;
        private bool _monitoring;
        private CoinChangeFinder _volumeCompare;

        public MonitorExchange()
        {
            _monitoring = false;
        }

        public bool AppliesTo(string message)
        {
            return message.StartsWith("m");
        }

        public MessageResponse Response(SocketMessage message)
        {
            if (!_monitoring)
            {
                StartMonitoring(message.Channel);
                return new MessageResponse("Started monitoring exchange.", false, null);
            } else
            {
                StopMonitoring();
                return new MessageResponse("Stopped monitoring exchange.", false, null);
            }

        }

        private void StartMonitoring(ISocketMessageChannel channel)
        {
            _volumeCompare = new CoinChangeFinder(channel);
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            _timer = new Timer(_volumeCompare.Run, autoEvent, 1000, 25000);
            _monitoring = true;
        }

        private void StopMonitoring()
        {
            _timer.Dispose();
            _monitoring = false;
            _volumeCompare.Stop();
        }
    }
}
