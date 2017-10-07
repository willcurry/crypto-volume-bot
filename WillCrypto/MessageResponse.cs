using Discord;

namespace WillCrypto
{
    internal class MessageResponse
    {
        public string Message { get; private set; }
        public bool TTS { get; private set; }
        public EmbedBuilder Embed  { get; private set; }

        public MessageResponse(string message, bool tts, EmbedBuilder embed)
        {
            Message = message;
            TTS = tts;
            Embed = embed;
        }
    }
}