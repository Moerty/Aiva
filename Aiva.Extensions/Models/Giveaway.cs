using Aiva.Extensions.Enums;

namespace Aiva.Extensions.Models {

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
