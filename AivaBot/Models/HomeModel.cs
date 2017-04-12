namespace AivaBot.Models {
    [PropertyChanged.ImplementPropertyChanged]
    class HomeModel {
        public string Header { get; set; }
        public MahApps.Metro.Controls.MetroContentControl Content { get; set; }
    }
}
