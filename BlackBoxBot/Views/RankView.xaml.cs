using System;
using System.Collections.Generic;
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

namespace BlackBoxBot.Controls {
	/// <summary>
	/// Interaktionslogik für ucRank.xaml
	/// </summary>
	public partial class ucRank : MahApps.Metro.Controls.MetroContentControl {
		public ucRank() {
			InitializeComponent();
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			Views.ModCommandLogViewer log = new Views.ModCommandLogViewer();
			log.Show();
		}
	}
}
