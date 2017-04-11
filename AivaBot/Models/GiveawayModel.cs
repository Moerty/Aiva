using System.Collections.ObjectModel;

namespace AivaBot.Models
{
    [PropertyChanged.ImplementPropertyChanged]
    public class GiveawayModel
    {
        public TextModel Text { get; set; }
        public GiveawayOptions Options { get; set; } = new GiveawayOptions();

        [PropertyChanged.ImplementPropertyChanged]
        public class GiveawayOptions {
            public bool Mod { get; set; } = true;
            public bool Sub { get; set; } = true;
            public bool Admin { get; set; } = true;
            public bool User { get; set; } = true;
            public int SubLuck { get; set; } = 1;
            public string Keyword { get; set; }
            public string Winner { get; set; }
        }
        
        public bool UncheckWinner { get; set; } = false;
        public ObservableCollection<string> Winners { get; set; }
        public bool IsStarted { get; set; } = false;

        [PropertyChanged.ImplementPropertyChanged]
        public class TextModel
        {
            public string CommandWatermark { get; set; }
            public string StatusInactive { get; set; }
            public string StatusActive { get; set; }
            public string SubLuckText { get; set; }
            public string ButtonGiveawayStart { get; set; }
            public string ButtonGiveawayStop { get; set; }
            public string ButtonGiveawayRoll { get; set; }
            public string ExpanderUsersDescription { get; set; }
            public string ExpanderWinnersDescription { get; set; }
            public string CheckBoxAdmin { get; set; }
            public string CheckBoxMod { get; set; }
            public string CheckBoxSub { get; set; }
            public string CheckBoxViewer { get; set; }
            public string UncheckWinner { get; set; }
        }
    }
}
