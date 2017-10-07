using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WillCrypto
{
    class Program
    {
        private DiscordSocketClient _client;
        private List<Command> _commands;
        
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        internal async Task MainAsync()
        {
            _commands = AllCommands.Get();

            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += MessageReceived;

            await _client.LoginAsync(TokenType.Bot, Settings.Token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage message)
        {
            foreach (Command command in _commands)
            {
                if (command.AppliesTo(message.Content))
                {
                    MessageResponse messageResponse = command.Response(message);
                    await message.Channel.SendMessageAsync(messageResponse.Message, messageResponse.TTS, messageResponse.Embed);
                }
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
