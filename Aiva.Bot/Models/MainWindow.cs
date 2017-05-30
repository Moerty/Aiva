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
    public class MainWindow : INotifyPropertyChanged {
        public ObservableCollection<TabItemsModel> TabItems { get; set; }
        public TabItemsModel SelectedTabItem { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public class TabItemsModel : INotifyPropertyChanged {
            public string Header { get; set; }
            public MetroContentControl Content { get; set; }
            public ObservableCollection<Flyout> Flyouts { get; set; }
            public WindowCommandsModel WindowCommands { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;


            public class WindowCommandsModel : INotifyPropertyChanged {
                public ObservableCollection<WindowCommandModel> RightWindowCommands { get; set; }
                public ObservableCollection<WindowCommandModel> LeftWindowCommands { get; set; }

                public event PropertyChangedEventHandler PropertyChanged;

                public class WindowCommandModel : INotifyPropertyChanged {
                    public string Header { get; set; }
                    public ICommand Command { get; set; }
                    public Visual Icon { get; set; }

                    public event PropertyChangedEventHandler PropertyChanged;
                }
            }
        }
    }
}
