using CefSharp;
using CefSharp.OffScreen;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using System.Threading;
using System.Collections.ObjectModel;
/*
 * - Setting who can request songs
    - max song duration
    - max quene size
    
    - cost sttings
    
    - add playlist
    - add video
 * */
namespace Aiva.Extensions.Songrequest
{
    public class Player
    {

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
        public ChromiumWebBrowser Browser { get; private set; }

        public bool IsInit { get; private set; }
        public bool IsMusicPlaying { get; private set; }
        public bool Autoplay { get; set; }
        public ObservableCollection<Song> SongList { get; set; }

        const string youtube = "https://www.youtube.com/embed/";

        public Player()
        {
            Browser = new ChromiumWebBrowser();
            YouTubeConnector = CreateYouTubeService();
            Browser.BrowserInitialized += BrowserInitialized;
            SongList = new ObservableCollection<Song>();
        }

        /// <summary>
        /// Internal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowserInitialized(object sender, System.EventArgs e)
        {
            IsInit = true;
        }

        /// <summary>
        /// Change Song
        /// </summary>
        /// <param name="autoplay"></param>
        /// <param name="song">todo: describe song parameter on ChangeSong</param>
        public void ChangeSong(Song song, bool autoplay = false)
        {
            string url = youtube + song.VideoID + (autoplay ? "?autoplay=1" : "");

            Browser.Load(url);

            if (autoplay) {
                IsMusicPlaying = true;
            }
        }

        /// <summary>
        /// Start/Stop the Player
        /// </summary>
        public void StartStopMusic()
        {
            if (IsMusicPlaying) {
                Browser.GetBrowser().GetHost().SendMouseClickEvent(0, 0, MouseButtonType.Left, false, 1, CefEventFlags.None);
                Thread.Sleep(100);
                Browser.GetBrowser().GetHost().SendMouseClickEvent(0, 0, MouseButtonType.Left, true, 1, CefEventFlags.None);
                IsMusicPlaying = false;
            }
        }

        /// <summary>
        /// Reset the Player to default;
        /// </summary>
        public void ResetPlayer()
        {
            Browser = new ChromiumWebBrowser();
        }


        /// <summary>
        /// Create the YouTube Service
        /// </summary>
        /// <returns></returns>
        public static YouTubeService CreateYouTubeService()
        {
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
                new FileDataStore("Aiva")
            ).Result;


            var youtubeService = new YouTubeService(new BaseClientService.Initializer {
                HttpClientInitializer = credential,
                ApplicationName = "Aiva"
            });

            return youtubeService;
        }
    }
}
