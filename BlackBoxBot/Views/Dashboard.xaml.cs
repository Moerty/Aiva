using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlackBoxBot.Views {
	/// <summary>
	/// Interaktionslogik für ucDashboard.xaml
	/// </summary>
	///

	public partial class Dashboard : MahApps.Metro.Controls.MetroContentControl {

		public Dashboard() {
			InitializeComponent();

			if (!DesignerProperties.GetIsInDesignMode(this)) LoadGamesFromTwitchAsync();
			cbCommercial.ItemsSource = Enum.GetValues(typeof(TwitchLib.Enums.CommercialLength));
		}

		private async void btnTitleChange_ClickAsync(object sender, RoutedEventArgs e)
		{
			if(!String.IsNullOrWhiteSpace(tbStreamtitel.Text))
			{
				await TwitchLib.TwitchApi.Streams.UpdateStreamTitleAsync(tbStreamtitel.Text, Config.General.Config["General"]["Channel"].ToLower());
			}
		}

		private async void LoadGamesFromTwitchAsync()
		{
			var games = await TwitchLib.TwitchApi.Games.GetGamesByPopularityAsync(100);

			foreach(var game in games)
			{
				cbGames.Items.Add(game.Game.Name);
			}
		}

		private async void btnGameChange_ClickAsync(object sender, RoutedEventArgs e)
		{
			await TwitchLib.TwitchApi.Streams.UpdateStreamGameAsync(cbGames.Text, Config.General.Config["General"]["Channel"].ToLower(), Config.General.Config["Credentials"]["TwitchOAuth"]);
		}

		private void btnSendMessage_Click(object sender, RoutedEventArgs e)
		{
			if (!String.IsNullOrEmpty(tbMessageToSend.Text) && !String.IsNullOrWhiteSpace(tbMessageToSend.Text))
			{
                Client.Client.ClientBBB.TwitchClientBBB.SendMessage(tbMessageToSend.Text);
				tbMessageToSend.Text = "";
			}
			else
				MessageBox.Show("Gebe eine Nachricht ein!");
		}

		private void tbMessageToSend_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
				btnSendMessage_Click(this, null);
		}

		private void tbStreamtitel_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Enter)
				btnTitleChange_ClickAsync(this, null);
		}

		private async void btnCommercial_ClickAsync(object sender, RoutedEventArgs e)
		{
			TwitchLib.Enums.CommercialLength result;

			if (Enum.TryParse(cbCommercial.Text, out result))
			{
				try
				{
					var resultString = await TwitchLib.TwitchApi.Streams.RunCommercialAsync(result, Config.General.Config["General"]["Channel"].ToLower(),
                                                                                        Config.General.Config["Credentials"]["TwitchOAuth"]);
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
			}
		}

		private void btnCommercial_ToolTipOpening(object sender, ToolTipEventArgs e)
		{
			btnCommercial.ToolTip = "Schaltet Werbung in der angegebenen Zeit" + Environment.NewLine +
										"Funktioniert nur wenn der Stream online ist!";
		}
	}
}
