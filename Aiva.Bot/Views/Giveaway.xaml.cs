using MahApps.Metro.Controls;

namespace Aiva.Bot.Views {

    /// <summary>
    /// Interaktionslogik für Giveaway.xaml
    /// </summary>
    public partial class Giveaway : MetroContentControl {

        public Giveaway() {
            InitializeComponent();
            this.DataContext = new ViewModels.Giveaway();
        }
    }
}