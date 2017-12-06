using Aiva.Models.Enums;
using Aiva.Models.Extensions;
using PropertyChanged;

namespace Aiva.Models.Voting {
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

        public JoinPermission JoinPermission { get; set; }
        public bool IsCostEnabled { get; set; }
        public int Cost { get; set; }
        public bool IsMinutesActive { get; set; }
        public int MinutesActive { get; set; }
        public bool BeFollower { get; set; }
        public bool BlockMultiRegister { get; set; }
        public Options Options { get; set; }

        public Properties() {
            JoinPermission = JoinPermission.Everyone;
            Cost = 100;
            BeFollower = true;
            BlockMultiRegister = true;
            MinutesActive = 10;
            Options = new Options();
        }
    }
}