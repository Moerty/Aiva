﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AivaBot.Models {
    [PropertyChanged.ImplementPropertyChanged]
    class SongrequestModel {
        public static Songrequest.Song playedSong;

        public bool Songrequest { get; set; } = true;
        public string OnOffSongrequestText { get; set; }
        public AsyncObservableCollection<Songrequest.Song> SongList { get; set; } = new AsyncObservableCollection<Songrequest.Song>();
        public Songrequest.Player Player { get; set; }
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
