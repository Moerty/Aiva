using Aiva.Extensions.Enums;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Extensions.Models {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public static class Voting {
        [PropertyChanged.AddINotifyPropertyChangedInterface]
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

            public JoinPermission JoinPermission { get; set; } = JoinPermission.Everyone;
            public bool IsCostEnabled { get; set; }
            public int Cost { get; set; } = 100;
            public bool IsMinutesActive { get; set; }
            public int MinutesActive { get; set; }
            public bool BeFollower { get; set; } = true;
            public bool BlockMultiRegister { get; set; } = true;
            public Options Options { get; set; }
        }

        [PropertyChanged.AddINotifyPropertyChangedInterface]
        public class Options {

            public OptionsMember Option1 { get; set; } = new OptionsMember();
            public OptionsMember Option2 { get; set; } = new OptionsMember();
            public OptionsMember Option3 { get; set; } = new OptionsMember();
            public OptionsMember Option4 { get; set; } = new OptionsMember();
            public OptionsMember Option5 { get; set; } = new OptionsMember();
            public OptionsMember Option6 { get; set; } = new OptionsMember();

            [PropertyChanged.AddINotifyPropertyChangedInterface]
            public class OptionsMember {
                public bool ActiveOption { get; set; }
                private string _Option;
                public string Option {
                    get {
                        return _Option;
                    } set {
                        ActiveOption = !String.IsNullOrEmpty(value);

                        _Option = value;
                    }
                }
            }
        }

        [PropertyChanged.AddINotifyPropertyChangedInterface]
        public class ChartValues {
            public ObservableValue Option1 { get; set; } = new ObservableValue(0);
            public ObservableCollection<string> Option1Usernames { get; set; }

            public ObservableValue Option2 { get; set; } = new ObservableValue(0);
            public ObservableCollection<string> Option2Usernames { get; set; }

            public ObservableValue Option3 { get; set; } = new ObservableValue(0);
            public ObservableCollection<string> Option3Usernames { get; set; }

            public ObservableValue Option4 { get; set; } = new ObservableValue(0);
            public ObservableCollection<string> Option4Usernames { get; set; }

            public ObservableValue Option5 { get; set; } = new ObservableValue(0);
            public ObservableCollection<string> Option5Usernames { get; set; }

            public ObservableValue Option6 { get; set; } = new ObservableValue(0);
            public ObservableCollection<string> Option6Usernames { get; set; }

        }
    }
}
