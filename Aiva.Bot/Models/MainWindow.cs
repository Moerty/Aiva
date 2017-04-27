using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;

namespace Aiva.Bot.Models {
    [PropertyChanged.ImplementPropertyChanged]
    public class MainWindow {
        public ObservableCollection<TabItemsModel> TabItems { get; set; }

        [PropertyChanged.ImplementPropertyChanged]
        public class TabItemsModel {
            public string Header { get; set; }
            public MetroContentControl Content { get; set; }
            public List<FlyoutItem> Flyouts { get; set; }
        }
    }
}
