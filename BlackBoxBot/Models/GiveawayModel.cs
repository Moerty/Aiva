namespace BlackBoxBot.Models
{
    [PropertyChanged.ImplementPropertyChanged]
    public class GiveawayModel
    {
        public TextModel Text { get; set; }




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
