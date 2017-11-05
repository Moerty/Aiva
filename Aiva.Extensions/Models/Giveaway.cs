using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Extensions.Models {

    [TypeConverter(typeof(Extensions.EnumDescriptionTypeConverter))]
    public enum JoinPermission {
        [Description(nameof(Everyone))]
        Everyone,

        [Description(nameof(Subscriber))]
        Subscriber,

        [Description("Mod")]
        Moderation,
    }

    public class Giveaway {
        public string Username { get; set; }
        public string UserID { get; set; }
        public bool IsSub { get; set; }


        public class Messages {
            public string Username { get; set; }
            public string Message { get; set; }
        }

        public class Properties {
            private string _Command;
            public string Command {
                get {
                    return _Command;
                }
                set {
                    _Command = value.StartsWith("!") ? value : "!" + value;
                    _Command = _Command.TrimEnd('!');
                }
            }


            public int Price { get; set; }
            public int Timer { get; set; }
            public int SubLuck { get; set; }
            public bool BeFollower { get; set; } = true;
            public bool NotifyWinner { get; set; } = true;
            public bool RemoveWinnerFromList { get; set; } = true;
            public bool BlockReEntry { get; set; } = true;
            public bool IsSubLuckActive { get; set; }
            public JoinPermission JoinPermission { get; set; }
        }
    }


}
