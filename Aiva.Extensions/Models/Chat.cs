using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Extensions.Models {

    public class Chat : INotifyPropertyChanged {

        public class Messages : INotifyPropertyChanged {
            public bool IsUserMod { get; set; }
            public bool IsUserSub { get; set; }
            public string Username { get; set; }
            public string TwitchID { get; set; }
            public string Message { get; set; }
            public DateTime TimeStamp { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public class Viewers : INotifyPropertyChanged {
            public bool IsMod { get; set; }
            public bool IsSub { get; set; }
            public string Name { get; set; }
            public string TwitchID { get; set; }
            public string Type { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        public enum SortDirectionListView {
            Admin,
            Mod,
            Subscriber,
            Follower,
            Viewer
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
