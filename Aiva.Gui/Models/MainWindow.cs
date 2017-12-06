using MahApps.Metro.Controls;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

namespace Aiva.Gui.Models {
    public class MainWindow {
        public List<TabItemsModel> TabItems { get; set; }
        public TabItemsModel SelectedTabItem { get; set; }

        public class TabItemsModel {
            public string Header { get; set; }
            public MetroContentControl Content { get; set; }
            public List<Flyout> Flyouts { get; set; }
            public WindowCommandsModel WindowCommands { get; set; }

            public class WindowCommandsModel {
                public List<WindowCommandModel> RightWindowCommands { get; set; }
                public List<WindowCommandModel> LeftWindowCommands { get; set; }

                public class WindowCommandModel {
                    public string Header { get; set; }
                    public ICommand Command { get; set; }
                    public Visual Icon { get; set; }
                }
            }
        }
    }
}