using LiveCharts.Defaults;

namespace AivaBot.Models {
    [PropertyChanged.ImplementPropertyChanged]
    class VotingModel {

        public bool IsStarted { get; set; } = false;
        public bool ChartPie { get; set; } = true;
        public bool ChartBasic { get; set; } = false;
        public bool ChartDonut { get; set; } = false;

        public string Command { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string Option6 { get; set; }
        public string Option7 { get; set; }
        public string Option8 { get; set; }

        public ObservableValue CountOption1 { get; set; } = new ObservableValue(0);
        public ObservableValue CountOption2 { get; set; } = new ObservableValue(0);
        public ObservableValue CountOption3 { get; set; } = new ObservableValue(0);
        public ObservableValue CountOption4 { get; set; } = new ObservableValue(0);
        public ObservableValue CountOption5 { get; set; } = new ObservableValue(0);
        public ObservableValue CountOption6 { get; set; } = new ObservableValue(0);
        public ObservableValue CountOption7 { get; set; } = new ObservableValue(0);
        public ObservableValue CountOption8 { get; set; } = new ObservableValue(0);

        public string Name { get; set; }

        public TextModel Text { get; set; }

        [PropertyChanged.ImplementPropertyChanged]
        public class TextModel {
            public string VotingButtonStartText { get; set; }
            public string VotingButtonStopText { get; set; }
            public string VotingNameWatermarkText { get; set; }
            public string VotingCommandWatermarkText { get; set; }
            public string VotingPieChatNameText { get; set; }
            public string VotingBarChartNameText { get; set; }
            public string VotingDoughnutChartNameText { get; set; }
        }
    }
}
