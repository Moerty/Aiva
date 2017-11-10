using MahApps.Metro.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

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