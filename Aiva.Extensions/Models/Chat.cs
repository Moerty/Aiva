using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Aiva.Extensions.Models {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Chat {

        public class Messages {
            public bool IsUserMod { get; set; }
            public bool IsUserSub { get; set; }
            public bool IsBroadcaster { get; set; }
            public string Username { get; set; }
            public string TwitchID { get; set; }
            public string Message { get; set; }
            public DateTime TimeStamp { get; set; }
            public Color Color { get; set; }
        }

        [PropertyChanged.AddINotifyPropertyChangedInterface]
        public class Viewers {
            public bool IsMod { get; set; }
            public bool IsSub { get; set; }
            public string Name { get; set; }
            public string TwitchID { get; set; }
            public string Type { get; set; }
            public Color ChatNameColor { get; set; }
        }

        public enum SortDirectionListView {
            Admin,
            Mod,
            Subscriber,
            Follower,
            Viewer
        }
    }
}
