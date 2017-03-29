using System.Windows;
using System.Windows.Controls;
using TwitchLib;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace BlackBoxBot.Controls.Flyout
{
	/// <summary>
	/// Interaktionslogik für ucBans.xaml
	/// </summary>
	public partial class ucBans : MahApps.Metro.Controls.Flyout
	{
		public static ObservableCollection<string> bannedUsers;
		private TwitchLib.Models.API.User.User user;
		private readonly Config.General generalConfig;
		public ucBans()
		{
			/*if (DesignerProperties.GetIsInDesignMode(this)) return;
			generalConfig = new Config.General();
			InitializeComponent();
			bannedUsers = new ObservableCollection<string>();
			if (DesignerProperties.GetIsInDesignMode(this)) return;
			loadBanListAsync();
			listView.ItemsSource = bannedUsers;
			bannedUsers.CollectionChanged += ChangeBannedUserItems;*/
		}

		private void ChangeBannedUserItems(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
				listView.ItemsSource = null;

			listView.ItemsSource = bannedUsers;
		}

		private async static void loadBanListAsync()
		{
			var result = await Client.Tasks.Tasks.LoadBlockedUserAsync();

			foreach(var users in result)
			{
				bannedUsers.Add(users.User.Name);
			}
		}



		private void btnBannen_Click(object sender, RoutedEventArgs e)
		{
			if(tbBannName.Text != "")
			{
				listView.SelectionChanged -= listView_SelectionChangedAsync;
                Client.Tasks.Tasks.BannUserAsync(tbBannName.Text);
				bannedUsers.Add(tbBannName.Text);
				tbBannName.Text = "";
				listView.SelectionChanged += listView_SelectionChangedAsync;
			}
		}

		private void btnUnbann_Click(object sender, RoutedEventArgs e)
		{

			if (listView.SelectedValue.ToString() != "")
			{
				listView.SelectionChanged -= listView_SelectionChangedAsync;
                Client.Tasks.Tasks.UnbanUser(listView.SelectedValue.ToString());

				bannedUsers.Remove(listView.SelectedValue.ToString());

				tbTwitchID.Text = "";
				tbCreatedAt.Text = "";
				tbName.Text = "";
				tbDisplayName.Text = "";

				listView.SelectionChanged += listView_SelectionChangedAsync;


			}

		}

		private async void listView_SelectionChangedAsync(object sender, SelectionChangedEventArgs e)
		{
			var user = await TwitchApi.Users.GetUserAsync(listView.SelectedValue.ToString());
			this.user = user;

			tbTwitchID.Text = user.Id.ToString();
			tbCreatedAt.Text = user.CreatedAt.ToString();
			tbName.Text = user.Name;
			tbDisplayName.Text = user.DisplayName;
		}
	}
}
