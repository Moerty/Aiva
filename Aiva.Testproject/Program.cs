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


            var root = TwitchLib.TwitchAPI.Root.v5.GetRoot(Core.AivaClient.Instance.OAuthKey).Result;




            Console.ReadKey();
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