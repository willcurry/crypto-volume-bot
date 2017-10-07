using System;
using System.Collections.Generic;
using System.Text;

namespace WillCrypto
{
    internal class AllCommands
    {
        public static List<Command> Get()
        {
            List<Command> commands = new List<Command>();
            commands.Add(new FindCoin());
            commands.Add(new MonitorExchange());
            commands.Add(new TrackCoin());
            return commands;
        }
    }
}
