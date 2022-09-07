using IniFile;

namespace rich_presence_linux
{
    public class Config
    {
        public Structs.GamesPlayerName PlayerNames;
        Ini ini;

        public Config()
        {
            try {
                ini = new Ini("Settings.ini");
                Console.WriteLine("Config file found!");
            } catch (FileNotFoundException) {
                Console.WriteLine("Config file not found, creating new config...");
                ini = new Ini
                {
                    new Section("PlayerName")
                    {
                        new Property("bf4", ""),
                        new Property("bf1", ""),
                        new Property("bf5", ""),
                    }
                };
                ini.SaveTo(@"Settings.ini");
            }


            Console.WriteLine("Current config:");
            Console.WriteLine($"Bf4 = {ini["PlayerName"]["bf4"]}");
            Console.WriteLine($"Bf1 = {ini["PlayerName"]["bf1"]}");
            Console.WriteLine($"Bf5 = {ini["PlayerName"]["bf5"]}");
            Refresh();
        }

        public void Refresh()
        {
            PlayerNames = new Structs.GamesPlayerName()
            {
                Bf4 = ini["PlayerName"]["bf4"].ToString(), 
                Bf1 = ini["PlayerName"]["bf1"].ToString(), 
                Bf5 = ini["PlayerName"]["bf5"].ToString(), 
            };
        }
    }
}