using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;


namespace Songrequest {
    public class GoogleCheck {
        public static bool Authenticate() {
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets {
                ClientId = "58340406586-ncdbbvr3p49gisgbjk5uu0vtfdfm5gsn.apps.googleusercontent.com",
                ClientSecret = "S0efx9ay11SH3Mly5HJ3zwVn"
            },
            new[] { YouTubeService.Scope.Youtube },
            "user",
            CancellationToken.None,
            new FileDataStore("AivaBot")).Result;

            var youtubeService = new YouTubeService(new BaseClientService.Initializer() {
                HttpClientInitializer = credential,
                ApplicationName = "AivaBot"
            });

            return credential != null;
        }
    }
}
