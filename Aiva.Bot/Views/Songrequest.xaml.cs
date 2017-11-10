using MahApps.Metro.Controls;

namespace Aiva.Bot.Views {

    /// <summary>
    /// Interaktionslogik für Songrequest.xaml
    /// </summary>
    public partial class Songrequest : MetroContentControl {

        public Songrequest() {
            InitializeComponent();
            this.DataContext = new ViewModels.Songrequest();
        }
    }
}