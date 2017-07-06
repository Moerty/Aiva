using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;

namespace Aiva.Bot.Models {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class MainWindow {
        public ObservableCollection<TabItemsModel> TabItems { get; set; }
        public TabItemsModel SelectedTabItem { get; set; }

        [PropertyChanged.AddINotifyPropertyChangedInterface]
        public class TabItemsModel {
            public string Header { get; set; }
            public MetroContentControl Content { get; set; }
            public ObservableCollection<Flyout> Flyouts { get; set; }
            public WindowCommandsModel WindowCommands { get; set; }


            [PropertyChanged.AddINotifyPropertyChangedInterface]
            public class WindowCommandsModel {
                public ObservableCollection<WindowCommandModel> RightWindowCommands { get; set; }
                public ObservableCollection<WindowCommandModel> LeftWindowCommands { get; set; }

                [PropertyChanged.AddINotifyPropertyChangedInterface]
                public class WindowCommandModel {
                    public string Header { get; set; }
                    public ICommand Command { get; set; }
                    public Visual Icon { get; set; }
                }
            }
        }
    }
}
