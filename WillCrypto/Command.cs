using Discord.WebSocket;

namespace WillCrypto
{
    internal interface Command
    {
        MessageResponse Response(SocketMessage message);
        bool AppliesTo(string message);
    }
}
