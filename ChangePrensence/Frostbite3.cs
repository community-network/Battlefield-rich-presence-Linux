using rich_presence_linux.Structs;
using DiscordRPC;

namespace rich_presence_linux.ChangePrensence
{
    public class Frostbite3
    {
        public static void Update(DiscordRpcClient client, DateTime startTime, GameInfo gameInfo, ServerInfo serverInfo)
        {
            serverInfo.MaxPlayers = serverInfo.MaxPlayers;
            string state = serverInfo.GetPlayerCountString();
            state += $" - {serverInfo.MapLabel}";

            //Set the rich presence
            //Call this as many times as you want and anywhere in your code.
            RichPresence presence = new RichPresence
            {
                Details = $"{serverInfo.Name}",
                State = state,
                Timestamps = new Timestamps
                {
                    Start = startTime
                },
                Assets = new Assets
                {
                    LargeImageKey = gameInfo.ShortName.ToLower(),
                    LargeImageText = gameInfo.FullName,
                    SmallImageKey = "gt",
                    SmallImageText = "Battlefield rich presence"
                }
            };

            List<Button> buttons = new List<Button>();

            String apiName = gameInfo.ShortName.ToLower();

            if (gameInfo.Game != Resources.Statics.Game.Bf5)
            {
                buttons.Add(new Button { Label = "Join", Url = $"https://joinme.click/g/{gameInfo.ShortName.ToLower()}/{serverInfo.GameId}" });
            } else
            {
                apiName = "bfv";
            }

            buttons.Add(new Button { Label = "View server", Url = $"https://gametools.network/servers/{apiName}/gameid/{serverInfo.GameId}/pc" });

            presence.Buttons = buttons.ToArray();
            client.SetPresence(presence);
        }
    }
}