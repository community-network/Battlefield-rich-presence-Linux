using Newtonsoft.Json;
using System.Net;
using rich_presence_linux.Structs;

namespace rich_presence_linux
{
    public class Api
    {
        public static ServerInfo OldTitleServerInfo(string unescapedPlayerName, string gameName)
        {
            WebClient webClient = new WebClient();
            string playerName = Uri.EscapeDataString(unescapedPlayerName);
            string data = webClient.DownloadString(new Uri($"https://api.bflist.io/{gameName}/v1/players/{playerName}/server"));
            return JsonConvert.DeserializeObject<ServerInfo>(data);
        }

        public static ServerInfo GetCurrentServer(string playerName, string ShortGameName)
        {
            var post = new
            {
                playerName
            };
            string dataString = JsonConvert.SerializeObject(post);
            WebClient webClient = new WebClient();
            string jwtData = Jwt.Create(dataString);
            webClient.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string postData = JsonConvert.SerializeObject(new { data = jwtData });
            string data = webClient.UploadString(new Uri($"https://api.gametools.network/currentserver/{ShortGameName}"), "POST", postData);
            if (data == "{}")
            {
                throw new Exception("not in a server");
            }
            return JsonConvert.DeserializeObject<ServerInfo>(data);
        }

    }
}