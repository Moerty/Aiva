using Aiva.Core.Config;
using System;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Input;

namespace Aiva.Bot.Models {
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
                    return Convert.ToBoolean(GeneralConfig.Config["SpamCheck"]["SkipMessageCheckAdmin"]);
                }
                set {
                    GeneralConfig.Config["SpamCheck"]["SkipMessageCheckAdmin"] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public bool SkipMessageCheckBroadcaster {
                get {
                    return Convert.ToBoolean(GeneralConfig.Config["SpamCheck"]["SkipMessageCheckBroadcaster"]);
                }
                set {
                    GeneralConfig.Config["SpamCheck"]["SkipMessageCheckBroadcaster"] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public bool SkipMessageCheckGlobalMod {
                get {
                    return Convert.ToBoolean(GeneralConfig.Config["SpamCheck"]["SkipMessageCheckGlobalMod"]);
                }
                set {
                    GeneralConfig.Config["SpamCheck"]["SkipMessageCheckGlobalMod"] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public bool SkipMessageCheckMod {
                get {
                    return Convert.ToBoolean(GeneralConfig.Config["SpamCheck"]["SkipMessageCheckMod"]);
                }
                set {
                    GeneralConfig.Config["SpamCheck"]["SkipMessageCheckMod"] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public bool SkipMessageCheckStaff {
                get {
                    return Convert.ToBoolean(GeneralConfig.Config["SpamCheck"]["SkipMessageCheckStaff"]);
                }
                set {
                    GeneralConfig.Config["SpamCheck"]["SkipMessageCheckStaff"] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public bool SkipMessageCheckViewer {
                get {
                    return Convert.ToBoolean(GeneralConfig.Config["SpamCheck"]["SkipMessageCheckViewer"]);
                }
                set {
                    GeneralConfig.Config["SpamCheck"]["SkipMessageCheckViewer"] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public TimeSpan TimeToNewMessage {
                get {
                    return TimeSpan.Parse(GeneralConfig.Config["SpamCheck"]["TimeToNewMessage"]);
                }
                set {
                    GeneralConfig.Config["SpamCheck"]["TimeToNewMessage"] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public TimeSpan MinutesTimeoutWarning {
                get {
                    return TimeSpan.Parse(GeneralConfig.Config["SpamCheck"]["MinutesTimeoutWarning"]);
                }
                set {
                    GeneralConfig.Config["SpamCheck"]["MinutesTimeoutWarning"] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public TimeSpan TimeoutTime {
                get {
                    return TimeSpan.Parse(GeneralConfig.Config["SpamCheck"]["TimeoutTime"]);
                }
                set {
                    GeneralConfig.Config["SpamCheck"]["TimeoutTime"] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public TimeSpan WarningTimeoutTime {
                get {
                    return TimeSpan.Parse(GeneralConfig.Config["SpamCheck"]["WarningTimeoutTime"]);
                }
                set {
                    GeneralConfig.Config["SpamCheck"]["WarningTimeoutTime"] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public TimeSpan TimeActiveWarning {
                get {
                    return TimeSpan.Parse(GeneralConfig.Config["SpamCheck"]["TimeActiveWarning"]);
                }
                set {
                    GeneralConfig.Config["SpamCheck"]["TimeActiveWarning"] = value.ToString();
                    GeneralConfig.WriteConfig();
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
                    GeneralConfig.Config["Credentials"]["EncryptedOAuth"] = value;
                    _TwitchOAuthKeyEncrypt = value;
                    GeneralConfig.WriteConfig();
                }
            }


            private string _TwitchOAuthDecrypt = "";
            public string TwitchOAuthDecrypt {
                get {
                    return _TwitchOAuthDecrypt;
                }
                set {
                    GeneralConfig.Config["Credentials"]["TwitchOAuth"] = value;
                    byte[] data = Encoding.UTF8.GetBytes(GeneralConfig.Config["Credentials"]["TwitchOAuth"]);
                    using (SHA512 shaM = new SHA512Managed()) {
                        byte[] result = shaM.ComputeHash(data);
                        _TwitchOAuthDecrypt = Convert.ToBase64String(result);
                    }
                    GeneralConfig.WriteConfig();
                }
            }

            public string TwitchClientID {
                get {
                    return GeneralConfig.Config["Credentials"]["TwitchClientID"];
                }
                set {
                    GeneralConfig.Config["Credentials"]["TwitchClientID"] = value;
                    GeneralConfig.WriteConfig();
                }
            }

            public string Channel {
                get {
                    return GeneralConfig.Config["General"]["Channel"];
                }
                set {
                    GeneralConfig.Config["General"]["Channel"] = value;
                    GeneralConfig.WriteConfig();
                }
            }

            public string BotName {
                get {
                    return GeneralConfig.Config["General"]["BotName"];
                }
                set {
                    GeneralConfig.Config["General"]["BotName"] = value;
                    GeneralConfig.WriteConfig();
                }
            }

            public string Language {
                get {
                    return GeneralConfig.Config["General"]["Language"];
                }
                set {
                    GeneralConfig.Config["General"]["Language"] = value;
                    GeneralConfig.WriteConfig();
                }
            }

            public char CommandIdentifier {
                get {
                    return Convert.ToChar(GeneralConfig.Config["General"]["CommandIdentifier"]);
                }
                set {
                    GeneralConfig.Config["General"]["CommandIdentifier"] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public int LogLevel {
                get {
                    return Convert.ToInt32(GeneralConfig.Config["General"]["LogLevel"]);
                }
                set {
                    GeneralConfig.Config["General"]["LogLevel"] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            // Currency
            public bool Active {
                get {
                    return Convert.ToBoolean(GeneralConfig.Config["Currency"]["Active"]);
                }
                set {
                    GeneralConfig.Config["Currency"]["Active"] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public int CurrencyToAdd {
                get {
                    return Convert.ToInt32(GeneralConfig.Config["Currency"]["CurrencyToAdd"]);
                }
                set {
                    GeneralConfig.Config["Currency"]["CurrencyToAdd"] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public TimeSpan TimerAddCurrency {
                get {
                    return TimeSpan.Parse(GeneralConfig.Config["Currency"]["TimerAddCurrency"]);
                }
                set {
                    GeneralConfig.Config["Currency"]["TimerAddCurrency"] = value.Ticks.ToString();
                    GeneralConfig.WriteConfig();
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
                        return Convert.ToBoolean(BankheistConfig.Config["General"]["Active"]);
                    }
                    set {
                        BankheistConfig.Config["General"]["Active"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                public string BankheistCommand {
                    get {
                        return BankheistConfig.Config["General"]["Command"];
                    }
                    set {
                        BankheistConfig.Config["General"]["Command"] = value;
                        BankheistConfig.WriteConfig();
                    }
                }
                public TimeSpan BankheistDuration {
                    get {
                        return TimeSpan.Parse(BankheistConfig.Config["General"]["BankheistTime"]);
                    }
                    set {
                        BankheistConfig.Config["General"]["BankheistTime"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }
                public TimeSpan BankheistPause {
                    get {
                        return TimeSpan.Parse(BankheistConfig.Config["General"]["TimeToNewBankheist"]);
                    }
                    set {
                        BankheistConfig.Config["General"]["TimeToNewBankheist"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                // Bank1
                public int Bank1SuccessRate {
                    get {
                        return Convert.ToInt32(BankheistConfig.Config["Bank1"]["SuccessRate"]);
                    }
                    set {
                        BankheistConfig.Config["Bank1"]["SuccessRate"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                public int Bank1MinimumPlayers {
                    get {
                        return Convert.ToInt32(BankheistConfig.Config["Bank1"]["MinimumPlayers"]);
                    }
                    set {
                        BankheistConfig.Config["Bank1"]["MinimumPlayers"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                public double Bank1WinningMultiplier {
                    get {
                        return Convert.ToDouble(BankheistConfig.Config["Bank1"]["WinningMultiplier"]);
                    }
                    set {
                        BankheistConfig.Config["Bank1"]["WinningMultiplier"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                // Bank2
                public int Bank2SuccessRate {
                    get {
                        return Convert.ToInt32(BankheistConfig.Config["Bank2"]["SuccessRate"]);
                    }
                    set {
                        BankheistConfig.Config["Bank2"]["SuccessRate"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                public int Bank2MinimumPlayers {
                    get {
                        return Convert.ToInt32(BankheistConfig.Config["Bank2"]["MinimumPlayers"]);
                    }
                    set {
                        BankheistConfig.Config["Bank2"]["MinimumPlayers"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                public double Bank2WinningMultiplier {
                    get {
                        return Convert.ToDouble(BankheistConfig.Config["Bank2"]["WinningMultiplier"]);
                    }
                    set {
                        BankheistConfig.Config["Bank2"]["WinningMultiplier"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                // Bank3
                public int Bank3SuccessRate {
                    get {
                        return Convert.ToInt32(BankheistConfig.Config["Bank3"]["SuccessRate"]);
                    }
                    set {
                        BankheistConfig.Config["Bank3"]["SuccessRate"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                public int Bank3MinimumPlayers {
                    get {
                        return Convert.ToInt32(BankheistConfig.Config["Bank3"]["MinimumPlayers"]);
                    }
                    set {
                        BankheistConfig.Config["Bank3"]["MinimumPlayers"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                public double Bank3WinningMultiplier {
                    get {
                        return Convert.ToDouble(BankheistConfig.Config["Bank3"]["WinningMultiplier"]);
                    }
                    set {
                        BankheistConfig.Config["Bank3"]["WinningMultiplier"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                // Bank4
                public int Bank4SuccessRate {
                    get {
                        return Convert.ToInt32(BankheistConfig.Config["Bank4"]["SuccessRate"]);
                    }
                    set {
                        BankheistConfig.Config["Bank4"]["SuccessRate"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                public int Bank4MinimumPlayers {
                    get {
                        return Convert.ToInt32(BankheistConfig.Config["Bank4"]["MinimumPlayers"]);
                    }
                    set {
                        BankheistConfig.Config["Bank4"]["MinimumPlayers"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                public double Bank4WinningMultiplier {
                    get {
                        return Convert.ToDouble(BankheistConfig.Config["Bank4"]["WinningMultiplier"]);
                    }
                    set {
                        BankheistConfig.Config["Bank4"]["WinningMultiplier"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                // Bank5
                public int Bank5SuccessRate {
                    get {
                        return Convert.ToInt32(BankheistConfig.Config["Bank5"]["SuccessRate"]);
                    }
                    set {
                        BankheistConfig.Config["Bank5"]["SuccessRate"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                public int Bank5MinimumPlayers {
                    get {
                        return Convert.ToInt32(BankheistConfig.Config["Bank5"]["MinimumPlayers"]);
                    }
                    set {
                        BankheistConfig.Config["Bank5"]["MinimumPlayers"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }

                public double Bank5WinningMultiplier {
                    get {
                        return Convert.ToDouble(BankheistConfig.Config["Bank5"]["WinningMultiplier"]);
                    }
                    set {
                        BankheistConfig.Config["Bank5"]["WinningMultiplier"] = value.ToString();
                        BankheistConfig.WriteConfig();
                    }
                }
            }
        }
    }
}

