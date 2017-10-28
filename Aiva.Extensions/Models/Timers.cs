using System;

namespace Aiva.Extensions.Models {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Timers {

        [PropertyChanged.AddINotifyPropertyChangedInterface]
        public class AddModel {
            public string Timer { get; set; }
            public string Text { get; set; }
            public DateTime CreatedAt { get; set; }
            public long Interval { get; set; } = 1;
            public bool Autoreset { get; set; }
            public bool Active { get; set; }
            public string SelectedUserRight { get; set; }
        }
    }
}
