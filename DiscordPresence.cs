using rich_presence_linux.ChangePrensence;
using rich_presence_linux.Resources;
using rich_presence_linux.Structs;
using DiscordRPC;

namespace rich_presence_linux
{
    public class DiscordPresence
    {
        private readonly Config _config;
        private DiscordRpcClient _client;
        private bool _discordIsRunning;
        private DateTime _startTime;
        private Statics.Game _previousGame;

        public DiscordPresence()
        {
            _config = new Config();
        }
        private void StartStopDiscord(GameInfo gameInfo)
        {
            if (gameInfo.IsRunning && !_discordIsRunning)
            {
                _client = new DiscordRpcClient(Statics.GameClientIds[gameInfo.Game]);
                _client.Initialize();
                _discordIsRunning = true;
                _startTime = DateTime.UtcNow.AddSeconds(1);
            }
            else if (!gameInfo.IsRunning && _discordIsRunning)
            {
                _client.Dispose();
                _discordIsRunning = false;
            // for weird edgecase where someone has 2 games running and quits one
            } else if (gameInfo.IsRunning && _discordIsRunning && _previousGame != gameInfo.Game)
            {
                _client.Dispose();
                _client = new DiscordRpcClient(Statics.GameClientIds[gameInfo.Game]);
                _client.Initialize();
                _startTime = DateTime.UtcNow.AddSeconds(1);
            }
        }

        private void UpdatePresenceInMenu(GameInfo gameInfo)
        {
            if (gameInfo.IsRunning && _discordIsRunning)
            {
                _client.SetPresence(new RichPresence
                {
                    Details = "In the menus",
                    State = "",
                    Timestamps = new Timestamps
                    {
                        Start = _startTime
                    },
                    Assets = new Assets
                    {
                        LargeImageKey = gameInfo.ShortName.ToLower(),
                        LargeImageText = gameInfo.FullName,
                        SmallImageKey = "gt",
                        SmallImageText = "Battlefield rich presence"
                    },

                });
            }
        }

        private void UpdatePresenceStatusUnknown(GameInfo gameInfo, string reason)
        {
            if (gameInfo.IsRunning && _discordIsRunning)
            {
                _client.SetPresence(new RichPresence
                {
                    Details = "Status unknown",
                    State = reason,
                    Timestamps = new Timestamps
                    {
                        Start = _startTime
                    },
                    Assets = new Assets
                    {
                        LargeImageKey = gameInfo.ShortName.ToLower(),
                        LargeImageText = gameInfo.FullName,
                        SmallImageKey = "gt",
                        SmallImageText = "Battlefield rich presence"
                    }
                });
            }
        }

        private void UpdatePresence(GameInfo gameInfo, ServerInfo serverInfo)
        {
            if (_discordIsRunning)
            {
                try
                {
                    if (Statics.Frostbite3Games.Contains(gameInfo.Game))
                    {
                        Frostbite3.Update(_client, _startTime, gameInfo, serverInfo);
                    } else
                    {
                        OlderTitles.Update(_client, _startTime, gameInfo, serverInfo);
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public void Update(object sender, System.Timers.ElapsedEventArgs e)
        {
            Update();
        }

        public void Update()
        {
            DateTime now = DateTime.Now;
            Console.WriteLine($"[{now}]: Send update to discord");

            GameInfo gameInfo = Game.IsRunning();
            StartStopDiscord(gameInfo);
            if (gameInfo.Game == Statics.Game.Bf5 || gameInfo.Game == Statics.Game.Bf1)
            {
                try
                {
                    var playerName = (string)_config.PlayerNames[gameInfo.ShortName];
                    ServerInfo serverInfo = Api.GetCurrentServer(playerName, gameInfo.ShortName.ToLower());
                    UpdatePresence(gameInfo, serverInfo);
                }
                catch (Exception)
                {
                    UpdatePresenceInMenu(gameInfo);
                }
            }
            else if (gameInfo.IsRunning && (string)_config.PlayerNames[gameInfo.ShortName] != null)
            {
                try
                {
                    var playerName = (string)_config.PlayerNames[gameInfo.ShortName];
                    ServerInfo serverInfo = Api.OldTitleServerInfo(playerName, gameInfo.ShortName.ToLower());
                    UpdatePresence(gameInfo, serverInfo);
                }
                catch (Exception)
                {
                    UpdatePresenceInMenu(gameInfo);
                }
            }
            else if (gameInfo.IsRunning)
            {
                UpdatePresenceStatusUnknown(gameInfo, "Playername not configured");
            }

            _previousGame = gameInfo.Game;
            _config.Refresh();
        }
    }
}