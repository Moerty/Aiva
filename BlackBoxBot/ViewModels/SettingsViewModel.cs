using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;

namespace BlackBoxBot.ViewModels {
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
        // TODO: Tabs
        private void CreateModels() {
            Model = new Models.SettingsModel();
            Model.SettingsTabs = new System.Collections.ObjectModel.ObservableCollection<Models.SettingsModel.SettingsTabItem> {
                new Models.SettingsModel.SettingsTabItem {
                    Header = "General",
                    Content = new Views.SettingsTabs.General(),
                },
                new Models.SettingsModel.SettingsTabItem {
                    Header = "Chat",
                    Content = new Views.SettingsTabs.Chat(),
                },
                new Models.SettingsModel.SettingsTabItem {
                    Header = "Minigames",
                    Content = new MahApps.Metro.Controls.MetroContentControl(),
                },
                new Models.SettingsModel.SettingsTabItem {
                    Header = "Commands",
                    Content = new MahApps.Metro.Controls.MetroContentControl(),
                },
            };
        }

        [PropertyChanged.ImplementPropertyChanged]
        public class ChatTabViewModel {
            public Models.SettingsModel.ChatTabModel Model { get; set; }

            public ICommand SaveCommand { get; set; } = new RoutedCommand();
            public ICommand AddBlacklistKeyword { get; set; } = new RoutedCommand();

            public ChatTabViewModel() {
                // Create Models
                CreateModels();

                // Commands
                var type = new MahApps.Metro.Controls.MetroContentControl().GetType();
                CommandManager.RegisterClassCommandBinding(type, new CommandBinding(SaveCommand, Save));
                CommandManager.RegisterClassCommandBinding(type, new CommandBinding(AddBlacklistKeyword, AddKeyword));
            }

            private void AddKeyword(object sender, ExecutedRoutedEventArgs e) {
                if(!Model.BlacklistedWords.Contains(Model.NewKeyword)) {
                    Model.BlacklistedWords.Add(Model.NewKeyword);
                    Model.NewKeyword = string.Empty;
                }
            }

            private void Save(object sender, ExecutedRoutedEventArgs e) {
                Database.UserSettingsHandler.WriteConfig(new List<Database.UserSettings> {
                    new Database.UserSettings {
                        Name = "BlacklistedWords",
                        Value = GetKeywordsFormattet()
                    },
                    new Database.UserSettings {
                        Name = "BlackListedWordsActive",
                        Value = Model.BlacklistedWordsActive.ToString()
                    },
                    new Database.UserSettings {
                        Name = "Spamcheck",
                        Value = Model.SpamCheck.ToString(),
                    },
                });

                // Update BlacklistedWords 4 
                Client.Tasks.ChatChecker.BlacklistedWords = Model.BlacklistedWords.ToList();

                if(Model.BlacklistedWordsActive) {
                    Client.Client.ClientBBB.TwitchClientBBB.OnMessageReceived += Client.Tasks.ChatChecker.BlacklistWordsChecker;
                } else {
                    Client.Client.ClientBBB.TwitchClientBBB.OnMessageReceived -= Client.Tasks.ChatChecker.BlacklistWordsChecker;
                }

                if(Model.SpamCheck) {
                    Client.Client.ClientBBB.TwitchClientBBB.OnMessageReceived += Client.Tasks.ChatChecker.CheckMessage;
                } else {
                    Client.Client.ClientBBB.TwitchClientBBB.OnMessageReceived -= Client.Tasks.ChatChecker.CheckMessage;
                }
            }

            string GetKeywordsFormattet() {
                StringBuilder sBuilder = new StringBuilder();
                foreach (var keyword in Model.BlacklistedWords) {
                    sBuilder.Append(keyword);
                    sBuilder.Append(',');
                }

                return sBuilder.ToString();
            }

            private void CreateModels() {
                var Settings = Database.UserSettingsHandler.GetConfig();

                Model = new Models.SettingsModel.ChatTabModel {
                    BlacklistedWords = new System.Collections.ObjectModel.ObservableCollection<string>(),
                    BlacklistedWordsActive = Convert.ToBoolean(Settings.Find(setting => String.Compare(setting.Name, "BlackListedWordsActive") == 0).Value),
                    SpamCheck = Convert.ToBoolean(Settings.Find(setting => String.Compare(setting.Name, "Spamcheck") == 0).Value),

                    Text = new Models.SettingsModel.ChatTabModel.TextModel {
                        ButtonSaveText = Config.Language.Instance.GetString("SettingsSaveButtonText"),
                    }
                };

                // Blacklisted Words
                var BlacklistedWords = Settings.SingleOrDefault(x => x.Name == "BlacklistedWords").Value.Split(',');
                foreach(var word in BlacklistedWords) {
                    Model.BlacklistedWords.Add(word);
                }
            }
        }

        /// <summary>
        /// Viewmodel from Settings General Tab
        /// </summary>
        [PropertyChanged.ImplementPropertyChanged]
        public class GeneralTabViewModel {
            public Models.SettingsModel.GeneralTabModel Model { get; set; }

            public GeneralTabViewModel() {

                string encryptedOAuthKey;
                byte[] data = Encoding.UTF8.GetBytes(Config.General.Config["Credentials"]["TwitchOAuth"]);
                using (SHA512 shaM = new SHA512Managed()) {
                    byte[] result = shaM.ComputeHash(data);
                    encryptedOAuthKey = Convert.ToBase64String(result);
                }

                Model = new Models.SettingsModel.GeneralTabModel {
                    TwitchOAuthKeyEncrypt = encryptedOAuthKey,
                    TwitchOAuthDecrypt = Config.General.Config["Credentials"]["TwitchOAuth"],
                    Channel = Config.General.Config["General"]["Channel"],
                    BotName = Config.General.Config["General"]["BotName"],
                    Language = Config.General.Config["General"]["Language"],
                    CommandIdentifier = Convert.ToChar(Config.General.Config["General"]["CommandIdentifier"]),
                    LogLevel = Convert.ToInt32(Config.General.Config["General"]["LogLevel"]),
                    Active = Convert.ToBoolean(Config.General.Config["Currency"]["Active"]),
                    CurrencyToAdd = Convert.ToInt32(Config.General.Config["Currency"]["CurrencyToAdd"]),
                    TimerAddCurrency = TimeSpan.Parse(Config.General.Config["Currency"]["TimerAddCurrency"]),
                };
            }
        }
    }
}
