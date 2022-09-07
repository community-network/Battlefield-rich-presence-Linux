using rich_presence_linux.Resources;

namespace rich_presence_linux.Structs
{
    public class GameInfo
    {
        public Statics.Game Game { get; set; }
        public bool IsRunning { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
    }
}