using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Extensions.Models {
    [PropertyChanged.ImplementPropertyChanged]
    public class Chat {

        [PropertyChanged.ImplementPropertyChanged]
        public class Messages {
            public bool IsUserMod { get; set; }
            public bool IsUserSub { get; set; }
            public string Username { get; set; }
            public long TwitchID { get; set; }
            public string Message { get; set; }
            public DateTime TimeStamp { get; set; }
        }

        [PropertyChanged.ImplementPropertyChanged]
        public class Viewers {
            public bool IsMod { get; set; }
            public bool IsSub { get; set; }
            public string Name { get; set; }
            public long TwitchID { get; set; }
        }
    }
}
