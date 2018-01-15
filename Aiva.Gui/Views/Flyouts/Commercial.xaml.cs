using MahApps.Metro.Controls;

namespace Aiva.Gui.Views.Flyouts {
    /// <summary>
    /// Interaktionslogik für ShowCommercial.xaml
    /// </summary>
    public partial class Commercial : Flyout
    {
        public Commercial()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.Flyouts.Commercial();
        }
    }
}
