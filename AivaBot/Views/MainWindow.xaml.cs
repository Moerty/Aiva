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

namespace AivaBot.Views
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MahApps.Metro.Controls.MetroWindow {
        public MainWindow() {
			InitializeComponent();
            this.DataContext = new ViewModels.MainViewModel();
		}
	}
}
