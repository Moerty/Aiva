using System.Collections.ObjectModel;

namespace Aiva.Bot.Models {
    [PropertyChanged.ImplementPropertyChanged]
    public class DashboardModel {

        public TextModel Text { get; set; }
        public ObservableCollection<Aiva.Extensions.Models.Dashboard.Followers> Followers { get; set; }
        public ObservableCollection<string> Games { get; set; }
        public string SelectedGame { get; set; }
        public string StreamTitle { get; set; }

        [PropertyChanged.ImplementPropertyChanged]
        public class TextModel {
            public string DashboardExpanderStatisticNameText { get; set; }
            public string DashboardExpanderFollowerNameText { get; set; }
            public string DashboardExpanderFollowerNameColumn { get; set; }
            public string DashboardExpanderFollowerCreatedAtColumn { get; set; }
            public string DashboardExpanderFollowerNotificationColumn { get; set; }
        }
    }
}
