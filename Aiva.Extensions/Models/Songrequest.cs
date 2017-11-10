using Aiva.Extensions.Enums;

namespace Aiva.Extensions.Models {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Songrequest {

        [PropertyChanged.AddINotifyPropertyChangedInterface]
        public class AddModel {
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
            public bool NotifyRequester { get; set; } = true;
            public bool RemoveSongAfterPlaying { get; set; } = true;
            public bool BlockMultiSong { get; set; } = true;
            public bool AutoStart { get; set; }
        }

        [PropertyChanged.AddINotifyPropertyChangedInterface]
        public class SongModel {
            public string Title { get; set; }
            public string Length { get; set; }
            public string Url { get; set; }
            public string Requester { get; set; }
            public string RequesterID { get; set; }
            public string VideoID { get; set; }
        }
    }
}