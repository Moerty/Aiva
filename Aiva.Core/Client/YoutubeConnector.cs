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

namespace Aiva.Core.Client {
    public class YoutubeConnector {

        /// <summary>
        /// Create the YouTube Service
        /// </summary>
        /// <returns></returns>
        public static YouTubeService CreateYouTubeService() {
            UserCredential credential;
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets {
                    ClientId = Config.Config.Instance["Credentials"]["GoogleClientID"],
                    ClientSecret = Config.Config.Instance["Credentials"]["GoogleClientSecret"],
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
