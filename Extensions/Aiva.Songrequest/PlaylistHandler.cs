using System;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using System.Threading;
using RestSharp.Extensions.MonoHttp;
using System.Collections.ObjectModel;
using Songrequest.Models;

namespace Songrequest {
    [PropertyChanged.ImplementPropertyChanged]
    public class PlaylistHandler {
        private readonly string ClientID;
        private readonly string ClientSecret;
        private YouTubeService youtubeService;
        public ObservableCollection<Models.VideoModel> Playlist { get; set; }

        /// <summary>
        /// Initialize Class
        /// FIRST STARTUP: CreatePlaylist()
        /// EVERY STARTUP: createYoutubeService
        /// </summary>
        /// <param name="youtubeCredentials">generelCredentials</param>
        /// <param name="songrequestConfig">songrequest</param>
        public PlaylistHandler(string clientid, string clientsecret) {
            this.ClientID = clientid;
            this.ClientSecret = clientsecret;

            createYoutubeService();

            if (Playlist == null)
                Playlist = new ObservableCollection<Models.VideoModel>();
        }

        private void createYoutubeService() {
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

            this.youtubeService = youtubeService;
        }

        public void RemoveVideoFromPlaylist(Song song) {
            var entry = Playlist.SingleOrDefault(x => x.VideoID == song.VideoID);


            if (entry != null) Playlist.Remove(entry);
        }

        public async Task<AddSongReturnModel> AddVideoToPlaylistAsync(string inputUser, string username) {
            var videoid = getVideoID(inputUser);

            // Check if Video exists || TODO
            if (!Playlist.Where(x => x.VideoID == videoid).Any()) {
                var request = youtubeService.Videos.List("snippet,contentDetails");
                request.Id = videoid;
                var response = request.Execute();

                // Found Video
                if (response.Items.Count == 1) {
                    var video = response.Items[0];

                    var videoModel = new Models.VideoModel {
                        VideoID = videoid,
                        Title = video.Snippet.Title,
                        Length = System.Xml.XmlConvert.ToTimeSpan(video.ContentDetails.Duration),
                        Username = username,
                    };

                    // Video is added
                    return new AddSongReturnModel { video = videoModel, returnModel = ReturnModel.Completed };
                }
                // Vid
                // Video not found
                else {
                    return new AddSongReturnModel { video = null, returnModel = ReturnModel.VideoNotFound };
                }
            }
            // Video already exists in Playlist
            else {
                return new AddSongReturnModel { video = null, returnModel = ReturnModel.VideoAlreadyExists };
            }
        }

        private static string getVideoID(string arg) {
            // https://www.youtube.com/watch?v=ARfqiQRSPFc
            if (arg.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) {
                var query = HttpUtility.ParseQueryString(new Uri(arg).Query);

                return query.AllKeys.Contains("v") ? query["v"] : new Uri(arg).Segments.Last();
            }

            // /watch?v=ARfqiQRSPFc
            return arg.StartsWith("/watch?", StringComparison.OrdinalIgnoreCase) ? arg.Substring(9, arg.Length - 9) : arg;
        }
    }
}
