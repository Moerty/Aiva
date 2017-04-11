using RestSharp;
using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;


namespace Songrequest {
    public class GoogleCheck
    {
		public static bool Authenticate()
		{
			var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
			{
				ClientId = "58340406586-ncdbbvr3p49gisgbjk5uu0vtfdfm5gsn.apps.googleusercontent.com",
				ClientSecret = "S0efx9ay11SH3Mly5HJ3zwVn"
            },
			new[] { YouTubeService.Scope.Youtube },
			"user",
			CancellationToken.None,
			new FileDataStore("AivaBot")).Result;


			var youtubeService = new YouTubeService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = "AivaBot"
			});

            return credential != null;
		}

		public static void GetToken()
		{
			var client = new RestClient("https://accounts.google.com/o/oauth2/auth");
			RestRequest request = new RestRequest() { Method = Method.POST };

			request.AddHeader("redirect_uri", "http://localhost");
			request.AddHeader("response_type", "token");
			request.AddHeader("scope", "https://www.googleapis.com/auth/youtube");
			request.AddHeader("client_id", "client_id=235778313506-9i7s8ghgtgn9v767murqk740i7febtho.apps.googleusercontent.com");

			var result = client.Execute(request);
		}

    }
}
