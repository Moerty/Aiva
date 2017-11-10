namespace Aiva.Bot.Views.Flyouts {

    /// <summary>
    /// Interaktionslogik für UserInfo.xaml
    /// </summary>
    ///
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public partial class UserInfo : MahApps.Metro.Controls.MetroContentControl {
        public new object DataContext { get; set; }

        public UserInfo(string name) {
            InitializeComponent();
            this.DataContext = new ViewModels.Flyouts.UsersInfoVM(name);
        }
    }
}