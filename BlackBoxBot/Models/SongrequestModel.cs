using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BlackBoxBot.Models
{
    [PropertyChanged.ImplementPropertyChanged]
    class SongrequestModel
    {
        public static Songrequest.Song playedSong; 

        public bool Songrequest { get; set; } = true;
        public string OnOffSongrequestText { get; set; }
        public Songrequest.PlaylistHandler Playlist { get; set; }
        //public ObservableCollection<Songrequest.Song> SongList { get; set; }
        public AsyncObservableCollection<Songrequest.Song> SongList { get; set; } = new AsyncObservableCollection<Songrequest.Song>();
        public Songrequest.Player Player { get; set; }

        public enum InformUser
        {
            AddedSuccessfully,
            VideoDuplicate,
            VideoNotFound,
            NotActivated
        }
    }
}
