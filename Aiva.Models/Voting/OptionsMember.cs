using PropertyChanged;

namespace Aiva.Models.Voting {
    [AddINotifyPropertyChangedInterface]
    public class OptionsMember {
        public bool ActiveOption { get; set; }
        private string _Option;
        public string Option {
            get {
                return _Option;
            }
            set {
                ActiveOption = !string.IsNullOrEmpty(value);

                _Option = value;
            }
        }
    }
}