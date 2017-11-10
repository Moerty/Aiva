namespace Aiva.Extensions.Models {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Commands {

        [PropertyChanged.AddINotifyPropertyChangedInterface]
        public class AddModel {
            public string Command { get; set; }
            public string Text { get; set; }
            public int Cooldown { get; set; }
            public UserRights SelectedUserRight { get; set; }
        }

        public enum UserRights {
            Viewer = 0,
            Mod = 1,
            Admin = 2
        }
    }
}