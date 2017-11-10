using Google.Apis.YouTube.v3;
using System;
using System.Linq;
using System.Web;

namespace Aiva.Extensions.Songrequest {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Song {
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public string VideoID { get; set; }
        public string UserInput { get; set; }
        public bool IsPlaying { get; set; }
        public string Url { get; set; }
        public bool FoundVideo;

        private YouTubeService _youTubeService;

        public Song(string UserInput) {
            this.UserInput = UserInput;
            _youTubeService = Core.Client.YoutubeConnector.CreateYouTubeService();
            VideoID = ExtractVideoID();
            Url = "https://www.youtube.com/watch?v=" + VideoID;

            GetVideoDetails();
        }

        /// <summary>
        /// Get Videodetails from YouTube
        /// </summary>
        private void GetVideoDetails() {
            var request = _youTubeService.Videos.List("snippet,contentDetails");
            request.Id = VideoID;
            var response = request.Execute();

            if (response.Items.Any()) {
                var Video = response.Items[0];

                Title = Video.Snippet.Title;
                Duration = System.Xml.XmlConvert.ToTimeSpan(Video.ContentDetails.Duration);

                FoundVideo = true;
            }
        }

        /// <summary>
        /// Extract the VideoID
        /// </summary>
        /// <returns></returns>
        private string ExtractVideoID() {
            // https://www.youtube.com/watch?v=ARfqiQRSPFc
            if (UserInput.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) {
                var query = HttpUtility.ParseQueryString(new Uri(UserInput).Query);
                return query.AllKeys.Contains("v") ? query["v"] : new Uri(UserInput).Segments.Last();
            }

            // /watch?v=ARfqiQRSPFc || VideoID
            return UserInput.StartsWith("/watch?", StringComparison.OrdinalIgnoreCase) ? UserInput.Substring(9, UserInput.Length - 9) : UserInput;
        }
    }
}