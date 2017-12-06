using LiveCharts.Defaults;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Gui.Models {
    [AddINotifyPropertyChangedInterface]
    public class ChartValues {
        public ObservableValue Option1 { get; set; }
        public ObservableValue Option2 { get; set; }
        public ObservableValue Option3 { get; set; }
        public ObservableValue Option4 { get; set; }
        public ObservableValue Option5 { get; set; }
        public ObservableValue Option6 { get; set; }

        public ChartValues() {
            Option1 = new ObservableValue(0);
            Option2 = new ObservableValue(0);
            Option3 = new ObservableValue(0);
            Option4 = new ObservableValue(0);
            Option5 = new ObservableValue(0);
            Option6 = new ObservableValue(0);
        }
    }
}
