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
            //public Dictionary<string, string> Settings { get; set; } = new Dictionary<string, string>();

            //private List<Database.UserSettings> _DatabaseEntrys;
            //public List<Database.UserSettings> DatabaseEntrys {
            //    get {
            //        if(_DatabaseEntrys == null) {
            //            _DatabaseEntrys = Database.UserSettingsHandler.GetConfig();
            //        }

            //        return _DatabaseEntrys;
            //    }
            //    set {
            //        DatabaseEntrys = value;
            //    }
            //}

            public ObservableCollection<string> BlacklistedWords { get; set; }

            private bool _BlacklistedWordsActive = false;
            public bool BlacklistedWordsActive {
                get {
                    return _BlacklistedWordsActive;
                }
                set {
                    if(_BlacklistedWordsActive) {
                        Client.Client.ClientBBB.TwitchClientBBB.OnMessageReceived -= Client.Tasks.ChatChecker.BlacklistWordsChecker;
                        _BlacklistedWordsActive = value;
                    }
                    else {
                        Client.Tasks.ChatChecker.BlacklistedWords = Database.UserSettingsHandler.GetBlacklistedWords();
                        Client.Client.ClientBBB.TwitchClientBBB.OnMessageReceived += Client.Tasks.ChatChecker.BlacklistWordsChecker;
                        _BlacklistedWordsActive = value;
                    }
                }
            }

            [PropertyChanged.ImplementPropertyChanged]
            public class TextModel {
                public string ButtonSaveText { get; set; }
            }
        }
    }
}

