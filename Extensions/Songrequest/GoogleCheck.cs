using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;


namespace Songrequest
{
    public class GoogleCheck
    {
		public static void Authenticate()
		{
			var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
			{
				ClientId = "235778313506-9i7s8ghgtgn9v767murqk740i7febtho.apps.googleusercontent.com",
				ClientSecret = "ttM4CVByzNNXO8AFmqe4kvC2"
			},
			new[] { YouTubeService.Scope.Youtube },
			"user",
			CancellationToken.None,
			new FileDataStore("BlackBoxBot")).Result;


			var youtubeService = new YouTubeService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = credential,
				ApplicationName = "BlackBoxBot"
			});
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
