using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using RestSharp.Extensions.MonoHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Songrequest {
    [PropertyChanged.ImplementPropertyChanged]
    public class Song {
        public string UserInput { get; set; }
        public string VideoID { get; set; }
        public string Username { get; set; }
        public Models.VideoModel VideoModel { get; set; }
        public bool IsPlaying { get; set; } = false;


        private YouTubeService youtubeService;
        private string ClientID;
        private string ClientSecret;
        private const string youtubeURL = "https://www.youtube.com/watch?v=";

        public Song(string userinput, string username, string clientID, string clientSecret) {
            UserInput = userinput;
            VideoID = ExtractVideoID();
            Username = username;
            ClientID = clientID;
            ClientSecret = clientSecret;
            youtubeService = CreateYouTubeService();
            VideoModel = GetVideoDetails();
        }

        private Models.VideoModel GetVideoDetails() {
            var request = youtubeService.Videos.List("snippet,contentDetails");
            request.Id = VideoID;
            var response = request.Execute();

            if (response.Items.Count == 1) {
                var video = response.Items[0];

                var videoModel = new Models.VideoModel {
                    VideoID = VideoID,
                    Title = video.Snippet.Title,
                    Length = System.Xml.XmlConvert.ToTimeSpan(video.ContentDetails.Duration),
                    Username = Username,
                    VideoStatus = Models.ReturnModel.Completed,
                    Url = youtubeURL + VideoID,
                };

                // Song correct play
                return videoModel;
            }
            // Video not found
            else {
                var videoModel = new Models.VideoModel {
                    VideoStatus = Models.ReturnModel.VideoNotFound,
                };
                return videoModel;
            }
        }


        private string ExtractVideoID() {
            // https://www.youtube.com/watch?v=ARfqiQRSPFc
            if (UserInput.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) {
                var query = HttpUtility.ParseQueryString(new Uri(UserInput).Query);
                return query.AllKeys.Contains("v") ? query["v"] : new Uri(UserInput).Segments.Last();
            }

            // /watch?v=ARfqiQRSPFc || VideoID
            return UserInput.StartsWith("/watch?", StringComparison.OrdinalIgnoreCase) ? UserInput.Substring(9, UserInput.Length - 9) : UserInput;
        }





        private YouTubeService CreateYouTubeService() {
            UserCredential credential;
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets {
                    ClientId = ClientID,
                    ClientSecret = ClientSecret
                },
                // This OAuth 2.0 access scope allows for full read/write access to the
                // authenticated user's account.
                new[] { YouTubeService.Scope.Youtube },
                "user",
                CancellationToken.None,
                new FileDataStore("AivaBot")
            ).Result;


            var youtubeService = new YouTubeService(new BaseClientService.Initializer {
                HttpClientInitializer = credential,
                ApplicationName = "AivaBot"
            });

            return youtubeService;
        }
    }
}
