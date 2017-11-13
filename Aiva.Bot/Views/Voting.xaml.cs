using MahApps.Metro.Controls;

namespace Aiva.Bot.Views {
    /// <summary>
    /// Interaktionslogik für Giveaway.xaml
    /// </summary>
    public partial class Voting : MetroContentControl {
        public Voting() {
            InitializeComponent();
            this.DataContext = new ViewModels.Voting();
        }
    }
}