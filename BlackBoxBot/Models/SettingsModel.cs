using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
            public TextModel Text { get; set; }
            public string NewKeyword { get; set; }

            public ObservableCollection<string> BlacklistedWords { get; set; }

            public bool BlacklistedWordsActive { get; set; }
            public bool SpamCheck { get; set; }

            [PropertyChanged.ImplementPropertyChanged]
            public class TextModel {
                public string ButtonSaveText { get; set; }
            }
        }
    }
}

