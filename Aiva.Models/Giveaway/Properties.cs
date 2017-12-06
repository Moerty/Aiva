using Aiva.Models.Enums;
using Aiva.Models.Extensions;
using PropertyChanged;

namespace Aiva.Models.Giveaway {
    [AddINotifyPropertyChangedInterface]
    public class Properties {
        private string _Command;

        public string Command {
            get {
                return _Command;
            }
            set {
                _Command = value.ModCommand();
            }
        }

        public int Price { get; set; }
        public int SubLuck { get; set; }
        public bool BeFollower { get; set; }
        public bool NotifyWinner { get; set; }
        public bool RemoveWinnerFromList { get; set; }
        public bool BlockReEntry { get; set; }
        public bool IsSubLuckActive { get; set; }
        public JoinPermission JoinPermission { get; set; }
        public bool TimerActive { get; set; }
        public int TimerValue { get; set; }

        public Properties() {
            JoinPermission = JoinPermission.Everyone;
            BeFollower = true;
            NotifyWinner = true;
            RemoveWinnerFromList = true;
            BlockReEntry = true;
        }
    }
}