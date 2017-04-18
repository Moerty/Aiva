using Aiva.Core.Client;
using Aiva.Core.Config;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aiva.Bot.Views {
    /// <summary>
    /// Interaktionslogik für ucDashboard.xaml
    /// </summary>

    public partial class Dashboard : MahApps.Metro.Controls.MetroContentControl {

        public Dashboard() {
            InitializeComponent();
            this.DataContext = new ViewModels.DashboardViewModel();
        }

        private async void btnCommercial_ClickAsync(object sender, RoutedEventArgs e) {
            TwitchLib.Enums.CommercialLength result;

            if (Enum.TryParse(cbCommercial.Text, out result)) {
                try {
                    var resultString = await TwitchLib.TwitchApi.Streams.RunCommercialAsync(result, GeneralConfig.Config["General"]["Channel"].ToLower(),
                                                                                        GeneralConfig.Config["Credentials"]["TwitchOAuth"]);
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void btnCommercial_ToolTipOpening(object sender, ToolTipEventArgs e) {
            btnCommercial.ToolTip = "Schaltet Werbung in der angegebenen Zeit" + Environment.NewLine +
                                        "Funktioniert nur wenn der Stream online ist!";
        }
    }
}
