using System;

namespace Aiva.Models.Songrequest {
    public class Song {
        public string Title { get; set; }
        public string Length { get; set; }
        public string LengthFormatted {
            get {
                var ts = TimeSpan.FromSeconds(Convert.ToInt32(Length));
                return ts.ToString();
            }
        }

        public string Url { get; set; }
        public string Requester { get; set; }
        public string RequesterID { get; set; }
        public string VideoID { get; set; }
        public string ImageUrl { get; set; }
        public string ViewCount { get; set; }
    }
}