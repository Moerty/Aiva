namespace Aiva.Bot.Views.Flyouts {

    /// <summary>
    /// Interaktionslogik für HonorSongrequester.xaml
    /// </summary>
    public partial class HonorSongrequester : MahApps.Metro.Controls.MetroContentControl {

        public HonorSongrequester(string username = null) {
            InitializeComponent();
            if (username == null)
                return;
            this.DataContext = new ViewModels.Flyouts.HonorSongrequester(null, null);
        }
    }
}