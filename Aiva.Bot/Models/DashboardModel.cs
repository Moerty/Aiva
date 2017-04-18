using System.Collections.ObjectModel;

namespace Aiva.Bot.Models {
    [PropertyChanged.ImplementPropertyChanged]
    public class DashboardModel {

        public TextModel Text { get; set; }
        public ObservableCollection<Aiva.Extensions.Models.Dashboard.Followers> Followers { get; set; }
        public ObservableCollection<string> Games { get; set; }
        public string SelectedGame { get; set; }
        public string StreamTitle { get; set; }
        public int TotalViews { get; set; }
        public int Viewers { get; set; }

        [PropertyChanged.ImplementPropertyChanged]
        public class TextModel {
            public string DashboardExpanderStatisticNameText { get; set; }
            public string DashboardExpanderFollowerNameText { get; set; }
            public string DashboardExpanderFollowerNameColumn { get; set; }
            public string DashboardExpanderFollowerCreatedAtColumn { get; set; }
            public string DashboardExpanderFollowerNotificationColumn { get; set; }
            public string DashboardLabelTitleText { get; set; }
            public string DashboardLabelGameText { get; set; }
            public string DashboardLabelCommercialText { get; set; }
            public string DashboardButtonTitleText { get; set; }
            public string DashboardButtonGameText { get; set; }
            public string DashboardLabelTotalViewsText { get; set; }
            public string DashboardLabelViewersCountText { get; set; }
        }
    }
}
