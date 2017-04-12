using System;
using System.IO;


namespace Songrequest.Models {
    [PropertyChanged.ImplementPropertyChanged]
    public class VideoModel {
        public string VideoID { get; set; }
        public string Title { get; set; }
        public TimeSpan Length { get; set; }
        public string DisplayImage { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Icons\Pause.png");
        public string Username { get; set; }
        public ReturnModel VideoStatus { get; set; }
        public string Url { get; set; }
    }

    public enum ReturnModel {
        NotDefinedError,
        VideoNotFound,
        Completed,
        VideoAlreadyExists
    }
}
