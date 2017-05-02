using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Aiva.Extensions.Songrequest
{
    public class Playlist
    {
        public string UserInput;
        public YouTubeService YouTubeConnector;

        public Playlist(string userinput)
        {
            YouTubeConnector = Player.CreateYouTubeService();
            this.UserInput = userinput;
        }
        /// <summary>
        /// Extract the VideoID
        /// </summary>
        /// <returns></returns>
        private string ExtractVideoID()
        {
            // https://www.youtube.com/watch?v=MpZFVM800f8&list=RDMpZFVM800f8#t=1
            if (UserInput.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) {
                var query = HttpUtility.ParseQueryString(new Uri(UserInput).Query);
                return query.AllKeys.Contains("list") ? query["list"] : new Uri(UserInput).Segments.Last();
            }

            // /watch?v=ARfqiQRSPFc || VideoID
            return UserInput.StartsWith("/watch?", StringComparison.OrdinalIgnoreCase) ? UserInput.Substring(9, UserInput.Length - 9) : UserInput;
        }

        public void GetYoutubeDetails()
        {
            var request = YouTubeConnector.PlaylistItems.List("snippet,contentDetails");
            //var request = YouTubeConnector.Videos.List("snippet,contentDetails");
            request.PlaylistId = UserInput;
            var response = request.Execute();

            if (response.Items.Any()) {
                foreach (var r in response.Items) {
                    Player.Instance.SongList.Add(
                        new Song(r.ContentDetails.VideoId, Core.AivaClient.Instance.Username));
                }
            }
        }
    }
}
