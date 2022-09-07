namespace rich_presence_linux
{
    class Program
    {

        static void Main(string[] args)
        {
            TimerPlus _timer = new TimerPlus();
            DiscordPresence discordPresence = new DiscordPresence();
            // run on startup instead of 15 seconds later
            discordPresence.Update();

            // background update interval
            _timer.Interval = 15000;
            _timer.Elapsed += discordPresence.Update;
            _timer.Start();
            Console.ReadKey();
        }
    }
}
