namespace Aiva.Models.Voting {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Options {
        public OptionsMember Option1 { get; set; }
        public OptionsMember Option2 { get; set; }
        public OptionsMember Option3 { get; set; }
        public OptionsMember Option4 { get; set; }
        public OptionsMember Option5 { get; set; }
        public OptionsMember Option6 { get; set; }

        public Options() {
            Option1 = new OptionsMember();
            Option2 = new OptionsMember();
            Option3 = new OptionsMember();
            Option4 = new OptionsMember();
            Option5 = new OptionsMember();
            Option6 = new OptionsMember();
        }
    }
}