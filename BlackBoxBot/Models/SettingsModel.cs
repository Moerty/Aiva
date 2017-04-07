using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
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

        [PropertyChanged.ImplementPropertyChanged]
        public class GeneralTabModel {

            public bool ShowTwitchOAuthKey { get; set; } = false;

            private string _TwitchOAuthKeyEncrypt = "";
            public string TwitchOAuthKeyEncrypt {
                get {
                    return _TwitchOAuthKeyEncrypt;
                }
                set {
                    Config.General.Config["Credentials"]["EncryptedOAuth"] = value;
                    _TwitchOAuthKeyEncrypt = value;
                    Config.General.WriteConfig();
                }
            }


            private string _TwitchOAuthDecrypt = "";
            public string TwitchOAuthDecrypt {
                get {
                    return _TwitchOAuthDecrypt;
                }
                set {
                    Config.General.Config["Credentials"]["TwitchOAuth"] = value;
                    byte[] data = Encoding.UTF8.GetBytes(Config.General.Config["Credentials"]["TwitchOAuth"]);
                    using (SHA512 shaM = new SHA512Managed()) {
                        byte[] result = shaM.ComputeHash(data);
                        _TwitchOAuthDecrypt = Convert.ToBase64String(result);
                    }
                    Config.General.WriteConfig();
                }
            }

            public string TwitchClientID {
                get {
                    return Config.General.Config["Credentials"]["TwitchClientID"];
                }
                set {
                    Config.General.Config["Credentials"]["TwitchClientID"] = value;
                    Config.General.WriteConfig();
                }
            }

            public string Channel {
                get {
                    return Config.General.Config["General"]["Channel"];
                }
                set {
                    Config.General.Config["General"]["Channel"] = value;
                    Config.General.WriteConfig();
                }
            }

            public string BotName {
                get {
                    return Config.General.Config["General"]["BotName"];
                }
                set {
                    Config.General.Config["General"]["BotName"] = value;
                    Config.General.WriteConfig();
                }
            }

            public string Language {
                get {
                    return Config.General.Config["General"]["Language"];
                }
                set {
                    Config.General.Config["General"]["Language"] = value;
                    Config.General.WriteConfig();
                }
            }

            public char CommandIdentifier {
                get {
                    return Convert.ToChar(Config.General.Config["General"]["CommandIdentifier"]);
                }
                set {
                    Config.General.Config["General"]["CommandIdentifier"] = value.ToString();
                    Config.General.WriteConfig();
                }
            }

            public int LogLevel {
                get {
                    return Convert.ToInt32(Config.General.Config["General"]["LogLevel"]);
                }
                set {
                    Config.General.Config["General"]["LogLevel"] = value.ToString();
                    Config.General.WriteConfig();
                }
            }

            public bool Active {
                get {
                    return Convert.ToBoolean(Config.General.Config["Currency"]["Active"]);
                }
                set {
                    Config.General.Config["Currency"]["Active"] = value.ToString();
                    Config.General.WriteConfig();
                }
            }

            public int CurrencyToAdd {
                get {
                    return Convert.ToInt32(Config.General.Config["Currency"]["CurrencyToAdd"]);
                }
                set {
                    Config.General.Config["Currency"]["CurrencyToAdd"] = value.ToString();
                    Config.General.WriteConfig();
                }
            }

            public TimeSpan TimerAddCurrency {
                get {
                    return TimeSpan.Parse(Config.General.Config["Currency"]["TimerAddCurrency"]);
                }
                set {
                    Config.General.Config["Currency"]["TimerAddCurrency"] = value.Ticks.ToString();
                    Config.General.WriteConfig();
                }
            }
        }
    }
}

