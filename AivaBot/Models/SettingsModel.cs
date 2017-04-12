using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;

namespace AivaBot.Models {
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

            //SpamCheck
            public bool SkipMessageCheckAdmin {
                get {
                    return Convert.ToBoolean(Config.General.Config["SpamCheck"]["SkipMessageCheckAdmin"]);
                }
                set {
                    Config.General.Config["SpamCheck"]["SkipMessageCheckAdmin"] = value.ToString();
                    Config.General.WriteConfig();
                }
            }

            public bool SkipMessageCheckBroadcaster {
                get {
                    return Convert.ToBoolean(Config.General.Config["SpamCheck"]["SkipMessageCheckBroadcaster"]);
                }
                set {
                    Config.General.Config["SpamCheck"]["SkipMessageCheckBroadcaster"] = value.ToString();
                    Config.General.WriteConfig();
                }
            }

            public bool SkipMessageCheckGlobalMod {
                get {
                    return Convert.ToBoolean(Config.General.Config["SpamCheck"]["SkipMessageCheckGlobalMod"]);
                }
                set {
                    Config.General.Config["SpamCheck"]["SkipMessageCheckGlobalMod"] = value.ToString();
                    Config.General.WriteConfig();
                }
            }

            public bool SkipMessageCheckMod {
                get {
                    return Convert.ToBoolean(Config.General.Config["SpamCheck"]["SkipMessageCheckMod"]);
                }
                set {
                    Config.General.Config["SpamCheck"]["SkipMessageCheckMod"] = value.ToString();
                    Config.General.WriteConfig();
                }
            }

            public bool SkipMessageCheckStaff {
                get {
                    return Convert.ToBoolean(Config.General.Config["SpamCheck"]["SkipMessageCheckStaff"]);
                }
                set {
                    Config.General.Config["SpamCheck"]["SkipMessageCheckStaff"] = value.ToString();
                    Config.General.WriteConfig();
                }
            }

            public bool SkipMessageCheckViewer {
                get {
                    return Convert.ToBoolean(Config.General.Config["SpamCheck"]["SkipMessageCheckViewer"]);
                }
                set {
                    Config.General.Config["SpamCheck"]["SkipMessageCheckViewer"] = value.ToString();
                    Config.General.WriteConfig();
                }
            }

            public TimeSpan TimeToNewMessage {
                get {
                    return TimeSpan.Parse(Config.General.Config["SpamCheck"]["TimeToNewMessage"]);
                }
                set {
                    Config.General.Config["SpamCheck"]["TimeToNewMessage"] = value.ToString();
                    Config.General.WriteConfig();
                }
            }

            public TimeSpan MinutesTimeoutWarning {
                get {
                    return TimeSpan.Parse(Config.General.Config["SpamCheck"]["MinutesTimeoutWarning"]);
                }
                set {
                    Config.General.Config["SpamCheck"]["MinutesTimeoutWarning"] = value.ToString();
                    Config.General.WriteConfig();
                }
            }

            public TimeSpan TimeoutTime {
                get {
                    return TimeSpan.Parse(Config.General.Config["SpamCheck"]["TimeoutTime"]);
                }
                set {
                    Config.General.Config["SpamCheck"]["TimeoutTime"] = value.ToString();
                    Config.General.WriteConfig();
                }
            }

            public TimeSpan WarningTimeoutTime {
                get {
                    return TimeSpan.Parse(Config.General.Config["SpamCheck"]["WarningTimeoutTime"]);
                }
                set {
                    Config.General.Config["SpamCheck"]["WarningTimeoutTime"] = value.ToString();
                    Config.General.WriteConfig();
                }
            }

            public TimeSpan TimeActiveWarning {
                get {
                    return TimeSpan.Parse(Config.General.Config["SpamCheck"]["TimeActiveWarning"]);
                }
                set {
                    Config.General.Config["SpamCheck"]["TimeActiveWarning"] = value.ToString();
                    Config.General.WriteConfig();
                }
            }

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

            // Currency
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


        [PropertyChanged.ImplementPropertyChanged]
        public class GamesTabModel {
            public BankheistModel Bankheist { get; set; }

            [PropertyChanged.ImplementPropertyChanged]
            public class BankheistModel {
                // General
                public bool BankheistActive {
                    get {
                        return Convert.ToBoolean(Config.Bankheist.Config["General"]["Active"]);
                    }
                    set {
                        Config.Bankheist.Config["General"]["Active"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                public string BankheistCommand {
                    get {
                        return Config.Bankheist.Config["General"]["Command"];
                    }
                    set {
                        Config.Bankheist.Config["General"]["Command"] = value;
                        Config.Bankheist.WriteConfig();
                    }
                }
                public TimeSpan BankheistDuration {
                    get {
                        return TimeSpan.Parse(Config.Bankheist.Config["General"]["BankheistTime"]);
                    }
                    set {
                        Config.Bankheist.Config["General"]["BankheistTime"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }
                public TimeSpan BankheistPause {
                    get {
                        return TimeSpan.Parse(Config.Bankheist.Config["General"]["TimeToNewBankheist"]);
                    }
                    set {
                        Config.Bankheist.Config["General"]["TimeToNewBankheist"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                // Bank1
                public int Bank1SuccessRate {
                    get {
                        return Convert.ToInt32(Config.Bankheist.Config["Bank1"]["SuccessRate"]);
                    }
                    set {
                        Config.Bankheist.Config["Bank1"]["SuccessRate"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                public int Bank1MinimumPlayers {
                    get {
                        return Convert.ToInt32(Config.Bankheist.Config["Bank1"]["MinimumPlayers"]);
                    }
                    set {
                        Config.Bankheist.Config["Bank1"]["MinimumPlayers"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                public double Bank1WinningMultiplier {
                    get {
                        return Convert.ToDouble(Config.Bankheist.Config["Bank1"]["WinningMultiplier"]);
                    }
                    set {
                        Config.Bankheist.Config["Bank1"]["WinningMultiplier"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                // Bank2
                public int Bank2SuccessRate {
                    get {
                        return Convert.ToInt32(Config.Bankheist.Config["Bank2"]["SuccessRate"]);
                    }
                    set {
                        Config.Bankheist.Config["Bank2"]["SuccessRate"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                public int Bank2MinimumPlayers {
                    get {
                        return Convert.ToInt32(Config.Bankheist.Config["Bank2"]["MinimumPlayers"]);
                    }
                    set {
                        Config.Bankheist.Config["Bank2"]["MinimumPlayers"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                public double Bank2WinningMultiplier {
                    get {
                        return Convert.ToDouble(Config.Bankheist.Config["Bank2"]["WinningMultiplier"]);
                    }
                    set {
                        Config.Bankheist.Config["Bank2"]["WinningMultiplier"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                // Bank3
                public int Bank3SuccessRate {
                    get {
                        return Convert.ToInt32(Config.Bankheist.Config["Bank3"]["SuccessRate"]);
                    }
                    set {
                        Config.Bankheist.Config["Bank3"]["SuccessRate"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                public int Bank3MinimumPlayers {
                    get {
                        return Convert.ToInt32(Config.Bankheist.Config["Bank3"]["MinimumPlayers"]);
                    }
                    set {
                        Config.Bankheist.Config["Bank3"]["MinimumPlayers"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                public double Bank3WinningMultiplier {
                    get {
                        return Convert.ToDouble(Config.Bankheist.Config["Bank3"]["WinningMultiplier"]);
                    }
                    set {
                        Config.Bankheist.Config["Bank3"]["WinningMultiplier"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                // Bank4
                public int Bank4SuccessRate {
                    get {
                        return Convert.ToInt32(Config.Bankheist.Config["Bank4"]["SuccessRate"]);
                    }
                    set {
                        Config.Bankheist.Config["Bank4"]["SuccessRate"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                public int Bank4MinimumPlayers {
                    get {
                        return Convert.ToInt32(Config.Bankheist.Config["Bank4"]["MinimumPlayers"]);
                    }
                    set {
                        Config.Bankheist.Config["Bank4"]["MinimumPlayers"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                public double Bank4WinningMultiplier {
                    get {
                        return Convert.ToDouble(Config.Bankheist.Config["Bank4"]["WinningMultiplier"]);
                    }
                    set {
                        Config.Bankheist.Config["Bank4"]["WinningMultiplier"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                // Bank5
                public int Bank5SuccessRate {
                    get {
                        return Convert.ToInt32(Config.Bankheist.Config["Bank5"]["SuccessRate"]);
                    }
                    set {
                        Config.Bankheist.Config["Bank5"]["SuccessRate"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                public int Bank5MinimumPlayers {
                    get {
                        return Convert.ToInt32(Config.Bankheist.Config["Bank5"]["MinimumPlayers"]);
                    }
                    set {
                        Config.Bankheist.Config["Bank5"]["MinimumPlayers"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }

                public double Bank5WinningMultiplier {
                    get {
                        return Convert.ToDouble(Config.Bankheist.Config["Bank5"]["WinningMultiplier"]);
                    }
                    set {
                        Config.Bankheist.Config["Bank5"]["WinningMultiplier"] = value.ToString();
                        Config.Bankheist.WriteConfig();
                    }
                }
            }
        }
    }
}

