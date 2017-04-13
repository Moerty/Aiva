using Aiva.Extensions.Songrequest;

namespace Aiva.Bot.Models {
    [PropertyChanged.ImplementPropertyChanged]
    class SongrequestModel {
        public static Song playedSong;

        public bool Songrequest { get; set; } = true;
        public string OnOffSongrequestText { get; set; }
        public AsyncObservableCollection<Song> SongList { get; set; } = new AsyncObservableCollection<Song>();
        public Player Player { get; set; }
        public TextModel Text { get; set; }

        public enum InformUser {
            AddedSuccessfully,
            VideoDuplicate,
            VideoNotFound,
            NotActivated
        }

        [PropertyChanged.ImplementPropertyChanged]
        public class TextModel {
            public string SongrequestCommandWatermarkText { get; set; }
            public string SongrequestExpanderRepeatText { get; set; }
            public string SongrequestButtonStartText { get; set; }
            public string SongrequestButtonStopText { get; set; }
            public string SongrequestButtonStopMusicText { get; set; }
        }
    }
}
