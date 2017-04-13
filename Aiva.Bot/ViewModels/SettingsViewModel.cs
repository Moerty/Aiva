using Aiva.Core.Client;
using Aiva.Core.Config;
using Aiva.Core.Database;
using Aiva.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    public class SettingsViewModel {
        private bool _LogChat;
        public bool LogChat {
            get {
                return _LogChat;
            }
            set {
                if (_LogChat) {
                    // Ausschalten
                    AivaClient.Client.AivaTwitchClient.OnMessageReceived -= ChatHandler.MessageReceivedAsync;
                    _LogChat = value;
                } else {
                    // Einschalten
                    AivaClient.Client.AivaTwitchClient.OnMessageReceived += ChatHandler.MessageReceivedAsync;
                    _LogChat = value;
                }
            }
        }

        public Models.SettingsModel Model { get; set; }

        public SettingsViewModel() {
            CreateModels();
        }
        // TODO: Tabs
        private void CreateModels() {
            Model = new Models.SettingsModel {
                SettingsTabs = new System.Collections.ObjectModel.ObservableCollection<Models.SettingsModel.SettingsTabItem> {
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
                    Content = new Views.SettingsTabs.Games(),
                },
                new Models.SettingsModel.SettingsTabItem {
                    Header = "Commands",
                    Content = new MahApps.Metro.Controls.MetroContentControl(),
                },
            }
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
                if (!Model.BlacklistedWords.Contains(Model.NewKeyword)) {
                    Model.BlacklistedWords.Add(Model.NewKeyword);
                    Model.NewKeyword = string.Empty;
                }
            }

            private void Save(object sender, ExecutedRoutedEventArgs e) {
                UserSettingsHandler.WriteConfig(new List<UserSettings> {
                    new UserSettings {
                        Name = "BlacklistedWords",
                        Value = GetKeywordsFormattet()
                    },
                    new UserSettings {
                        Name = "BlackListedWordsActive",
                        Value = Model.BlacklistedWordsActive.ToString()
                    },
                    new UserSettings {
                        Name = "Spamcheck",
                        Value = Model.SpamCheck.ToString(),
                    },
                });

                // Update BlacklistedWords 4
                Core.Client.Tasks.ChatChecker.BlacklistedWords = Model.BlacklistedWords.ToList();

                if (Model.BlacklistedWordsActive) {
                    AivaClient.Client.AivaTwitchClient.OnMessageReceived += Core.Client.Tasks.ChatChecker.BlacklistWordsChecker;
                } else {
                    AivaClient.Client.AivaTwitchClient.OnMessageReceived -= Core.Client.Tasks.ChatChecker.BlacklistWordsChecker;
                }

                if (Model.SpamCheck) {
                    AivaClient.Client.AivaTwitchClient.OnMessageReceived += Core.Client.Tasks.ChatChecker.CheckMessage;
                } else {
                    AivaClient.Client.AivaTwitchClient.OnMessageReceived -= Core.Client.Tasks.ChatChecker.CheckMessage;
                }
            }

            string GetKeywordsFormattet() {
                var sBuilder = new StringBuilder();
                foreach (var keyword in Model.BlacklistedWords) {
                    sBuilder.Append(keyword);
                    sBuilder.Append(',');
                }

                return sBuilder.ToString();
            }

            private void CreateModels() {
                var Settings = UserSettingsHandler.GetConfig();

                Model = new Models.SettingsModel.ChatTabModel {
                    BlacklistedWords = new System.Collections.ObjectModel.ObservableCollection<string>(),
                    BlacklistedWordsActive = Convert.ToBoolean(Settings.Find(setting => String.Compare(setting.Name, "BlackListedWordsActive") == 0).Value),
                    SpamCheck = Convert.ToBoolean(Settings.Find(setting => String.Compare(setting.Name, "Spamcheck") == 0).Value),

                    // SpamCheck Settings
                    SkipMessageCheckAdmin = Convert.ToBoolean(GeneralConfig.Config["SpamCheck"]["SkipMessageCheckAdmin"]),
                    SkipMessageCheckBroadcaster = Convert.ToBoolean(GeneralConfig.Config["SpamCheck"]["SkipMessageCheckBroadcaster"]),
                    SkipMessageCheckGlobalMod = Convert.ToBoolean(GeneralConfig.Config["SpamCheck"]["SkipMessageCheckGlobalMod"]),
                    SkipMessageCheckMod = Convert.ToBoolean(GeneralConfig.Config["SpamCheck"]["SkipMessageCheckMod"]),
                    SkipMessageCheckStaff = Convert.ToBoolean(GeneralConfig.Config["SpamCheck"]["SkipMessageCheckStaff"]),
                    SkipMessageCheckViewer = Convert.ToBoolean(GeneralConfig.Config["SpamCheck"]["SkipMessageCheckViewer"]),
                    TimeToNewMessage = TimeSpan.Parse(GeneralConfig.Config["SpamCheck"]["TimeToNewMessage"]),
                    MinutesTimeoutWarning = TimeSpan.Parse(GeneralConfig.Config["SpamCheck"]["MinutesTimeoutWarning"]),
                    TimeoutTime = TimeSpan.Parse(GeneralConfig.Config["SpamCheck"]["TimeoutTime"]),
                    WarningTimeoutTime = TimeSpan.Parse(GeneralConfig.Config["SpamCheck"]["WarningTimeoutTime"]),
                    TimeActiveWarning = TimeSpan.Parse(GeneralConfig.Config["SpamCheck"]["TimeActiveWarning"]),

                    Text = new Models.SettingsModel.ChatTabModel.TextModel {
                        ButtonSaveText = LanguageConfig.Instance.GetString("SettingsSaveButtonText"),
                    }
                };

                // Blacklisted Words
                var BlacklistedWords = Settings.SingleOrDefault(x => x.Name == "BlacklistedWords").Value.Split(',');
                foreach (var word in BlacklistedWords) {
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
                var data = Encoding.UTF8.GetBytes(GeneralConfig.Config["Credentials"]["TwitchOAuth"]);
                using (SHA512 shaM = new SHA512Managed()) {
                    var result = shaM.ComputeHash(data);
                    encryptedOAuthKey = Convert.ToBase64String(result);
                }

                Model = new Models.SettingsModel.GeneralTabModel {
                    TwitchOAuthKeyEncrypt = encryptedOAuthKey,
                    TwitchOAuthDecrypt = GeneralConfig.Config["Credentials"]["TwitchOAuth"],
                    Channel = GeneralConfig.Config["General"]["Channel"],
                    BotName = GeneralConfig.Config["General"]["BotName"],
                    Language = GeneralConfig.Config["General"]["Language"],
                    CommandIdentifier = Convert.ToChar(GeneralConfig.Config["General"]["CommandIdentifier"]),
                    LogLevel = Convert.ToInt32(GeneralConfig.Config["General"]["LogLevel"]),
                    Active = Convert.ToBoolean(GeneralConfig.Config["Currency"]["Active"]),
                    CurrencyToAdd = Convert.ToInt32(GeneralConfig.Config["Currency"]["CurrencyToAdd"]),
                    TimerAddCurrency = TimeSpan.Parse(GeneralConfig.Config["Currency"]["TimerAddCurrency"]),
                };
            }
        }

        /// <summary>
        /// Viewmodel from Settings Games Tab
        /// </summary>
        [PropertyChanged.ImplementPropertyChanged]
        public class GamesTabViewModel {
            public Models.SettingsModel.GamesTabModel Model { get; set; }

            public GamesTabViewModel() {
                CreateModel();
            }

            private void CreateModel() {
                Model = new Models.SettingsModel.GamesTabModel {
                    Bankheist = new Models.SettingsModel.GamesTabModel.BankheistModel {
                        BankheistActive = Convert.ToBoolean(BankheistConfig.Config["General"]["Active"]),
                        BankheistCommand = BankheistConfig.Config["General"]["Command"],
                        BankheistDuration = TimeSpan.Parse(BankheistConfig.Config["General"]["BankheistTime"]),
                        BankheistPause = TimeSpan.Parse(BankheistConfig.Config["General"]["TimeToNewBankheist"]),

                        // Bank1
                        Bank1SuccessRate = Convert.ToInt32(BankheistConfig.Config["Bank1"]["SuccessRate"]),
                        Bank1MinimumPlayers = Convert.ToInt32(BankheistConfig.Config["Bank1"]["MinimumPlayers"]),
                        Bank1WinningMultiplier = Convert.ToDouble(BankheistConfig.Config["Bank1"]["WinningMultiplier"]),

                        // Bank2
                        Bank2SuccessRate = Convert.ToInt32(BankheistConfig.Config["Bank2"]["SuccessRate"]),
                        Bank2MinimumPlayers = Convert.ToInt32(BankheistConfig.Config["Bank2"]["MinimumPlayers"]),
                        Bank2WinningMultiplier = Convert.ToDouble(BankheistConfig.Config["Bank2"]["WinningMultiplier"]),

                        // Bank3
                        Bank3SuccessRate = Convert.ToInt32(BankheistConfig.Config["Bank3"]["SuccessRate"]),
                        Bank3MinimumPlayers = Convert.ToInt32(BankheistConfig.Config["Bank3"]["MinimumPlayers"]),
                        Bank3WinningMultiplier = Convert.ToDouble(BankheistConfig.Config["Bank3"]["WinningMultiplier"]),

                        // Bank4
                        Bank4SuccessRate = Convert.ToInt32(BankheistConfig.Config["Bank4"]["SuccessRate"]),
                        Bank4MinimumPlayers = Convert.ToInt32(BankheistConfig.Config["Bank4"]["MinimumPlayers"]),
                        Bank4WinningMultiplier = Convert.ToDouble(BankheistConfig.Config["Bank4"]["WinningMultiplier"]),

                        // Bank5
                        Bank5SuccessRate = Convert.ToInt32(BankheistConfig.Config["Bank4"]["SuccessRate"]),
                        Bank5MinimumPlayers = Convert.ToInt32(BankheistConfig.Config["Bank4"]["MinimumPlayers"]),
                        Bank5WinningMultiplier = Convert.ToDouble(BankheistConfig.Config["Bank4"]["WinningMultiplier"]),
                    }
                };
            }
        }
    }
}
