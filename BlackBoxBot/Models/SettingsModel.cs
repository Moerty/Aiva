using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace BlackBoxBot.Models {
    [PropertyChanged.ImplementPropertyChanged]
    public class SettingsModel {

        public ObservableCollection<SettingsTabItem> SettingsTabs { get; set; }
        public ChatTabModel ChatTab { get; set; }

        [PropertyChanged.ImplementPropertyChanged]
        public class SettingsTabItem {
            public string Header { get; set; }
            public ICommand Command { get; set; } = new RoutedCommand();
            public MahApps.Metro.Controls.MetroContentControl Content { get; set; }
        }

        [PropertyChanged.ImplementPropertyChanged]
        public class ChatTabModel {
            private List<Database.UserSettings> _DatabaseEntrys;
            public List<Database.UserSettings> DatabaseEntrys {
                get {
                    if(_DatabaseEntrys == null) {
                        _DatabaseEntrys = Database.UserSettingsHandler.GetConfig();
                    }

                    return _DatabaseEntrys;
                }
            }
        }
    }
}
