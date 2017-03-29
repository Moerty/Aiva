using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TwitchLib;
using CefSharp;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace BlackBoxBot.Views
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MahApps.Metro.Controls.MetroWindow {
		private Controls.Main.ucDashboard dashboard;
		private Controls.Main.ucSettings settings;
        public MainWindow() {
			InitializeComponent();
		}

		/// <summary>
		/// Open Dashboard
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnDashboard_Click(object sender, RoutedEventArgs e)
		{
			tControlMain.Visibility = Visibility.Hidden;

			if (dashboard == null)
				dashboard = new Controls.Main.ucDashboard();

			if(!mainGrid.Children.Contains(dashboard))
				mainGrid.Children.Add(dashboard);

			Grid.SetRow(dashboard, 1);
		}

		/// <summary>
		/// Open Settings
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSettings_Click(object sender, RoutedEventArgs e)
		{
			tControlMain.Visibility = Visibility.Hidden;


			if (settings == null)
				settings = new Controls.Main.ucSettings();

			if (!mainGrid.Children.Contains(settings))
				mainGrid.Children.Add(settings);
			Grid.SetRow(settings, 1);
		}

		/// <summary>
		/// Open Home
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnHome_Click(object sender, RoutedEventArgs e)
		{
			tControlMain.Visibility = Visibility.Visible;

			if (dashboard != null)
				mainGrid.Children.Remove(dashboard);

			if (settings != null)
				mainGrid.Children.Remove(settings);

		}

		private void btnUsers_Click(object sender, RoutedEventArgs e)
		{
			var u = new Views.pUsers();
			u.Show();
		}
	}
}
