using PropertyChanged;
using System.Drawing;

namespace Aiva.Models.Chat {
    [AddINotifyPropertyChangedInterface]
    public class Viewer {
        public bool IsMod { get; set; }
        public bool IsSub { get; set; }
        public string Name { get; set; }
        public string TwitchID { get; set; }
        public string Type { get; set; }
        public Color ChatNameColor { get; set; }
    }
}