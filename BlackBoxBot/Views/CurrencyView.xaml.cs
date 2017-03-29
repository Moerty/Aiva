using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using Database;
using System.Collections.Generic;

namespace BlackBoxBot.Views
{
	/// <summary>
	/// Interaktionslogik für ucCurrency.xaml
	/// </summary>
	public partial class ucCurrency : MahApps.Metro.Controls.MetroContentControl {
        

        public ucCurrency() {
			InitializeComponent();
            this.DataContext = new ViewModels.CurrencyViewModel();
		}

        private void MetroContentControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            System.Windows.Data.CollectionViewSource currencyViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("currencyViewSource")));

            currencyViewSource.Source = Database.CurrencyHandler.GetCurrencyList();
        }
    }
}
