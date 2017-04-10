using System.Windows;

namespace BlackBoxBot.Controls {
    /// <summary>
    /// Interaktionslogik für ucRank.xaml
    /// </summary>
    public partial class Rank : MahApps.Metro.Controls.MetroContentControl {
        public Rank() {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e) {
            Views.ModCommandLogViewer log = new Views.ModCommandLogViewer();
            log.Show();
        }
    }
}