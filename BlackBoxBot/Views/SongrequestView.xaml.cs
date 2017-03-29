using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace BlackBoxBot.Views
{
	/// <summary>
	/// Interaktionslogik für ucSongrequest.xaml
	/// </summary>
	public partial class ucSongrequest : MahApps.Metro.Controls.MetroContentControl {
        public ucSongrequest() {
			InitializeComponent();
            this.DataContext = new ViewModels.SongrequestViewModel(this);
		}
    }
}