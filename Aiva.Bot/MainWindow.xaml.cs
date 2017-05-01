using MahApps.Metro.Controls;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Linq;

namespace Aiva.Bot {
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow {
        public MainWindow() {
            InitializeComponent();
            this.DataContext = new ViewModels.MainWindow();

            //var x = (this.DataContext as ViewModels.MainWindow).Model.TabItems[0].Flyouts[0].is
        }
    }

    //public class BoolToVisibilityConverter : IValueConverter {
    //    public object Convert(object value, Type targetType,
    //        object parameter, CultureInfo culture) {
    //        // Do the conversion from bool to visibility
    //    }

    //    public object ConvertBack(object value, Type targetType,
    //        object parameter, CultureInfo culture) {
    //        // Do the conversion from visibility to bool
    //    }
    
}
