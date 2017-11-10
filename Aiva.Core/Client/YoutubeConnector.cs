using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using System.Threading;

namespace Aiva.Core.Client {
    public class YoutubeConnector {
        private const string ClientId = "235778313506-9i7s8ghgtgn9v767murqk740i7febtho.apps.googleusercontent.com";
        private const string ClientSecret = "ttM4CVByzNNXO8AFmqe4kvC2";

        /// <summary>
        /// Create the YouTube Service
        /// </summary>
        /// <returns></returns>
        public static YouTubeService CreateYouTubeService() {
            UserCredential credential;
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets {
                    ClientId = ClientId,
                    ClientSecret = ClientSecret
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
    }
}