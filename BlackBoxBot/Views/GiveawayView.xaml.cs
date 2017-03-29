using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Giveaway;
using Giveaway.Models;

namespace BlackBoxBot.Views
{
	/// <summary>
	/// Interaktionslogik für cGiveaway.xaml
	/// </summary>
	public partial class Giveaway : MahApps.Metro.Controls.MetroContentControl
	{

		public Giveaway()
		{
            InitializeComponent();
            this.DataContext = new ViewModels.GiveawayViewModel();
		}

		private void BtnUncheckWinner_Click(object sender, RoutedEventArgs e)
		{
			/*var context = this.DataContext as Models.GiveawayModel;

			if(context.UncheckWinner)
			{
				context.UncheckWinner = false;
			}
			else
			{
				context.UncheckWinner = true;
			}*/
		}

		private void btnShowUsers_Click(object sender, RoutedEventArgs e)
		{
			//this.UserBansFlyoutControl.Visibility = Visibility.Visible;

			
			//this.UsersFlyout.Visibility = Visibility.Visible;
			//this.UsersFlyout.IsOpen = true;

			// Fix Designer
			//this.lvUsers.Visibility = Visibility.Visible;
		}
	}
}
