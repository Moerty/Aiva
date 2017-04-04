using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackBoxBot.ViewModels
{
    [PropertyChanged.ImplementPropertyChanged]
    class SettingsViewModel
    {
        private bool _LogChat = false;
        public bool LogChat
        {
            get
            {
                return _LogChat;
            }
            set
            {
                if(_LogChat)
                {
                    // Ausschalten
                    Client.Client.ClientBBB.TwitchClientBBB.OnMessageReceived -= Database.ChatHandler.MessageReceived;
                    _LogChat = value;
                }
                else
                {
                    // Einschalten
                    Client.Client.ClientBBB.TwitchClientBBB.OnMessageReceived += Database.ChatHandler.MessageReceived;
                    _LogChat = value;
                }
            }
        }

        public Models.SettingsModel Model { get; set; }

        public SettingsViewModel()
        {
            CreateModels();
        }

        private void CreateModels() {
            Model = new Models.SettingsModel();
            Model.SettingsTabs = new System.Collections.ObjectModel.ObservableCollection<Models.SettingsModel.SettingsTabItem> {
                new Models.SettingsModel.SettingsTabItem {
                    Header = "Chat",
                    Content = new Views.SettingsTabs.Chat(),
                }
            };

        }

        [PropertyChanged.ImplementPropertyChanged]
        public class ChatTabViewModel {
            public Models.SettingsModel.ChatTabModel Model { get; set; }

            public ChatTabViewModel() {
                // Create Models
                CreateModels();
            }

            private void CreateModels() {
                Model = new Models.SettingsModel.ChatTabModel {
                    //BlacklistedWords = Database.UserSettingsHandler.GetConfig("BlacklistedWords")
                };
            }
        }

    }
}
