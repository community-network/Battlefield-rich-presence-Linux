using System.Diagnostics;
using System.Text.RegularExpressions;
using rich_presence_linux.Resources;
using rich_presence_linux.Structs;

namespace rich_presence_linux
{
    public class Game
    {

        public static GameInfo IsRunning()
        {
            Process[] processCollection = Process.GetProcesses();
            foreach (Process p in processCollection)
            {
                Regex rgx = new Regex(Statics.SupportedGamesRegex, RegexOptions.IgnoreCase);
                Match match = rgx.Match(p.ProcessName);
                if (match.Success)
                {
                    string[] names = rgx.GetGroupNames();
                    foreach (var name in names)
                    {
                        Group grp = match.Groups[name];
                        if (!Int32.TryParse(name, out _) && grp.Value != "")
                        {
                            Statics.Game game = (Statics.Game)Enum.Parse(typeof(Statics.Game), name);
                            return new GameInfo
                            {
                                Game = game,
                                IsRunning = true,
                                ShortName = Statics.ShortGameName[game],
                                FullName = Statics.FullGameName[game]
                            };
                        }
                    }
                }
            }
            return new GameInfo
            {
                IsRunning = false,
                ShortName = "",
                FullName = ""
            };
        }
    }
}