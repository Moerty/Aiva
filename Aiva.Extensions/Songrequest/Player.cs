using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using System.Threading;
using System.Collections.ObjectModel;
using System;

namespace Aiva.Extensions.Songrequest {
    [PropertyChanged.ImplementPropertyChanged]
    public class Player {

        private static Player _Player;
        public static Player Instance {
            get {
                if (_Player == null)
                    _Player = new Player();

                return _Player;
            }
            private set {
                _Player = value;
            }
        }


        public YouTubeService YouTubeConnector { get; private set; }
        public bool Autoplay { get; set; } = true;
        public ObservableCollection<Song> SongList { get; set; }
        public Song SelectedSong { get; set; }
        public Song PlayedSong { get; set; }

        const string youtube = "https://www.youtube.com/embed/";

        public Player() {
            YouTubeConnector = CreateYouTubeService();
            SongList = new ObservableCollection<Song>();
        }

        /// <summary>
        /// Create the YouTube Service
        /// </summary>
        /// <returns></returns>
        public static YouTubeService CreateYouTubeService() {
            UserCredential credential;
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets {
                    ClientId = Core.Config.Config.Instance["Credentials"]["GoogleClientID"],
                    ClientSecret = Core.Config.Config.Instance["Credentials"]["GoogleClientSecret"],
                },
                // This OAuth 2.0 access scope allows for full read/write access to the
                // authenticated user's account.
                new[] { YouTubeService.Scope.Youtube },
                "user",
                CancellationToken.None,
                new FileDataStore(nameof(Aiva))
            ).Result;


            var youtubeService = new YouTubeService(new BaseClientService.Initializer {
                HttpClientInitializer = credential,
                ApplicationName = nameof(Aiva)
            });

            return youtubeService;
        }

        /// <summary>
        /// Delete Song from Playlist
        /// </summary>
        public void DeleteSongFromPlaylist() {
            SongList.Remove(SelectedSong);
            SelectedSong = null;
        }
    }
}
