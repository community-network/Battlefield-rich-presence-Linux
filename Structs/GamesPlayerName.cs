using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rich_presence_linux.Structs
{
    public class GamesPlayerName
    {
        public string Bf4 { get; set; }
        public string Bf1 { get; set; }
        public string Bf5 { get; set; }

        public object this[string propertyName]
        {
            get => GetType().GetProperty(propertyName)?.GetValue(this);
            set => GetType().GetProperty(propertyName)?.SetValue(this, value, null);
        }
    }
}