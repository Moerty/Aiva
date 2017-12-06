using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aiva.Extensions.Songrequest {
    public class YouTubeInfo {
        private const string Host = "https://youtube.com/";
        private const string VideoDetailsUri = "https://www.youtube.com/get_video_info?video_id=@videoid&sts=@ssts8&el=info&ps=default&hl=en";

        public readonly string VideoID;

        public YouTubeInfo(string videoid) {
            this.VideoID = videoid;
        }

        public async Task<Models.Songrequest.Song> GetVideoDetails() {
            var context = await GetPlayerContext().ConfigureAwait(false);

            var uri = VideoDetailsUri
                .Replace("@videoid", VideoID)
                .Replace("@ssts", context.Item2);

            var result = await GetContentAsString(uri).ConfigureAwait(false);

            return GetSongModel(result);
        }

        private Models.Songrequest.Song GetSongModel(string result) {
            var songModel = new Models.Songrequest.Song();
            var content = result.Split('&');
            foreach (var param in content) {
                // Look for the equals sign
                var equalsPos = param.IndexOf('=');
                if (equalsPos <= 0)
                    continue;

                // Get the key
                var key = param.Substring(0, equalsPos);
                var value = equalsPos < param.Length
                    ? param.Substring(equalsPos + 1)
                    : string.Empty;

                if (String.Compare(key, "title", true) == 0) {
                    songModel.Title = value;
                } else if (String.Compare(key, "iurl", true) == 0) {
                    songModel.ImageUrl = value;
                } else if (String.Compare(key, "length_seconds", true) == 0) {
                    songModel.Length = value;
                } else if (String.Compare(key, "view_count", true) == 0) {
                    songModel.ViewCount = value;
                }
            }

            if (String.IsNullOrEmpty(songModel.Title)
                || String.IsNullOrEmpty(songModel.ImageUrl)
                || String.IsNullOrEmpty(songModel.ViewCount)
                || String.IsNullOrEmpty(songModel.Length)) {
                return null;
            } else {
                songModel.VideoID = VideoID;
                return songModel;
            }
        }

        private async Task<Tuple<string, string>> GetPlayerContext() {
            var request = $"{Host}/embed/{VideoID}";

            var content = await GetContentAsString(request).ConfigureAwait(false);

            return new Tuple<string, string>(
                Regex.Match(content, @"""js""\s*:\s*""(.*?)""").Groups[1].Value.Replace("\\", ""),
                Regex.Match(content, @"""sts""\s*:\s*(\d+)").Groups[1].Value);
        }

        private async Task<string> GetContentAsString(string url) {
            using (var client = new HttpClient()) {
                var response = await client.GetAsync(url).ConfigureAwait(false);
                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
        }
    }
}