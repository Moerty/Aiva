using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aiva.Testproject {
    class Program {
        //static Aiva.Extensions.Songrequest.Player p;
        public static YouTubeService YouTubeConnector { get; private set; }


        static void Main(string[] args) {

            

            //p.Browser.Load("https://www.youtube.com/embed/bKXIUQbardg?autoplay=1");

            // Make sure you set performDependencyCheck false
            //Cef.Initialize();
            YouTubeConnector = CreateYouTubeService();
            //Extensions.Songrequest.Playlist p = new Extensions.Songrequest.Playlist("RDMpZFVM800f8");

            //YoutubePlayerLib.Cef.CefYoutubeController controller = new YoutubePlayerLib.Cef.CefYoutubeController();
            //controller. = "bKXIUQbardg";

            //Core.AivaClient.Instance.AivaTwitchClient.SendMessage("Aiva started.");
            //TwitchLib.TwitchApi.ValidationAPIRequest("pcwfd4rhcevonwdjw6kdh1g5f8bz1g");
            //TwitchLib.TwitchApi.ValidClientId("10n39mbfftkcy2kg1jkzmm62yszdcg");

            //TwitchLib.TwitchApi.SetAccessToken("5to3wi9u0bkxl6r1w9bhjpcq8r1dut");


            //var xy = TwitchLib.TwitchApi.Channels.GetChannelsObject(Core.AivaClient.Instance.TwitchID.ToString());

            //// https://www.youtube.com/watch?v=KbNXnxwMOqU
            //p = new Extensions.Songrequest.Player();

            //p.Browser.BrowserInitialized += Browser_BrowserInitialized;
            //p.ChangeSong(new Extensions.Songrequest.Song("5_SLU1ByyKg", "aeffchaen") {
            //    VideoID = "5_SLU1ByyKg",
            //}, true);



            Console.ReadKey();
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