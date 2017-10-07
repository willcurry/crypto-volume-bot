namespace WillCrypto
{
    internal class Settings
    {
        // Track requirements are checked at 25 second intervals and needs the % change to meet these requirements to start being tracked.
        public static double VolumeTrackRequirement = 0.4;
        public static double PriceTrackRequirement = 0.025;
        // Strike requirements are checked at 40 second intervals and needs the % change to meet these requirements or it will be striked.
        public static double PriceStrikeRequirement = 0.01;
        public static double VolumeStrikeRequirement = 0.1;
        // Token for your bot.
        public static string Token = "";
    }
}
