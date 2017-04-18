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
            public bool AllowViewerToPostLinks { get; set; }
            public bool CapsRestriction {
                get {
                    return Convert.ToBoolean(GeneralConfig.Config[nameof(SpamCheck)][nameof(CapsRestriction)]);
                }
                set {
                    GeneralConfig.Config[nameof(SpamCheck)][nameof(CapsRestriction)] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            //SpamCheck
            public bool SkipMessageCheckAdmin {
                get {
                    return Convert.ToBoolean(GeneralConfig.Config[nameof(SpamCheck)][nameof(SkipMessageCheckAdmin)]);
                }
                set {
                    GeneralConfig.Config[nameof(SpamCheck)][nameof(SkipMessageCheckAdmin)] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public bool SkipMessageCheckBroadcaster {
                get {
                    return Convert.ToBoolean(GeneralConfig.Config[nameof(SpamCheck)][nameof(SkipMessageCheckBroadcaster)]);
                }
                set {
                    GeneralConfig.Config[nameof(SpamCheck)][nameof(SkipMessageCheckBroadcaster)] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public bool SkipMessageCheckGlobalMod {
                get {
                    return Convert.ToBoolean(GeneralConfig.Config[nameof(SpamCheck)][nameof(SkipMessageCheckGlobalMod)]);
                }
                set {
                    GeneralConfig.Config[nameof(SpamCheck)][nameof(SkipMessageCheckGlobalMod)] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public bool SkipMessageCheckMod {
                get {
                    return Convert.ToBoolean(GeneralConfig.Config[nameof(SpamCheck)][nameof(SkipMessageCheckMod)]);
                }
                set {
                    GeneralConfig.Config[nameof(SpamCheck)][nameof(SkipMessageCheckMod)] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public bool SkipMessageCheckStaff {
                get {
                    return Convert.ToBoolean(GeneralConfig.Config[nameof(SpamCheck)][nameof(SkipMessageCheckStaff)]);
                }
                set {
                    GeneralConfig.Config[nameof(SpamCheck)][nameof(SkipMessageCheckStaff)] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public bool SkipMessageCheckViewer {
                get {
                    return Convert.ToBoolean(GeneralConfig.Config[nameof(SpamCheck)][nameof(SkipMessageCheckViewer)]);
                }
                set {
                    GeneralConfig.Config[nameof(SpamCheck)][nameof(SkipMessageCheckViewer)] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public TimeSpan TimeToNewMessage {
                get {
                    return TimeSpan.Parse(GeneralConfig.Config[nameof(SpamCheck)][nameof(TimeToNewMessage)]);
                }
                set {
                    GeneralConfig.Config[nameof(SpamCheck)][nameof(TimeToNewMessage)] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public TimeSpan MinutesTimeoutWarning {
                get {
                    return TimeSpan.Parse(GeneralConfig.Config[nameof(SpamCheck)][nameof(MinutesTimeoutWarning)]);
                }
                set {
                    GeneralConfig.Config[nameof(SpamCheck)][nameof(MinutesTimeoutWarning)] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public TimeSpan TimeoutTime {
                get {
                    return TimeSpan.Parse(GeneralConfig.Config[nameof(SpamCheck)][nameof(TimeoutTime)]);
                }
                set {
                    GeneralConfig.Config[nameof(SpamCheck)][nameof(TimeoutTime)] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public TimeSpan WarningTimeoutTime {
                get {
                    return TimeSpan.Parse(GeneralConfig.Config[nameof(SpamCheck)][nameof(WarningTimeoutTime)]);
                }
                set {
                    GeneralConfig.Config[nameof(SpamCheck)][nameof(WarningTimeoutTime)] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public TimeSpan TimeActiveWarning {
                get {
                    return TimeSpan.Parse(GeneralConfig.Config[nameof(SpamCheck)][nameof(TimeActiveWarning)]);
                }
                set {
                    GeneralConfig.Config[nameof(SpamCheck)][nameof(TimeActiveWarning)] = value.ToString();
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
                    var data = Encoding.UTF8.GetBytes(GeneralConfig.Config["Credentials"]["TwitchOAuth"]);
                    using (SHA512 shaM = new SHA512Managed()) {
                        var result = shaM.ComputeHash(data);
                        _TwitchOAuthDecrypt = Convert.ToBase64String(result);
                    }
                    GeneralConfig.WriteConfig();
                }
            }

            public string TwitchClientID {
                get {
                    return GeneralConfig.Config["Credentials"][nameof(TwitchClientID)];
                }
                set {
                    GeneralConfig.Config["Credentials"][nameof(TwitchClientID)] = value;
                    GeneralConfig.WriteConfig();
                }
            }

            public string Channel {
                get {
                    return GeneralConfig.Config["General"][nameof(Channel)];
                }
                set {
                    GeneralConfig.Config["General"][nameof(Channel)] = value;
                    GeneralConfig.WriteConfig();
                }
            }

            public string BotName {
                get {
                    return GeneralConfig.Config["General"][nameof(BotName)];
                }
                set {
                    GeneralConfig.Config["General"][nameof(BotName)] = value;
                    GeneralConfig.WriteConfig();
                }
            }

            public string Language {
                get {
                    return GeneralConfig.Config["General"][nameof(Language)];
                }
                set {
                    GeneralConfig.Config["General"][nameof(Language)] = value;
                    GeneralConfig.WriteConfig();
                }
            }

            public char CommandIdentifier {
                get {
                    return Convert.ToChar(GeneralConfig.Config["General"][nameof(CommandIdentifier)]);
                }
                set {
                    GeneralConfig.Config["General"][nameof(CommandIdentifier)] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public int LogLevel {
                get {
                    return Convert.ToInt32(GeneralConfig.Config["General"][nameof(LogLevel)]);
                }
                set {
                    GeneralConfig.Config["General"][nameof(LogLevel)] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            // Currency
            public bool Active {
                get {
                    return Convert.ToBoolean(GeneralConfig.Config["Currency"][nameof(Active)]);
                }
                set {
                    GeneralConfig.Config["Currency"][nameof(Active)] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public int CurrencyToAdd {
                get {
                    return Convert.ToInt32(GeneralConfig.Config["Currency"][nameof(CurrencyToAdd)]);
                }
                set {
                    GeneralConfig.Config["Currency"][nameof(CurrencyToAdd)] = value.ToString();
                    GeneralConfig.WriteConfig();
                }
            }

            public TimeSpan TimerAddCurrency {
                get {
                    return TimeSpan.Parse(GeneralConfig.Config["Currency"][nameof(TimerAddCurrency)]);
                }
                set {
                    GeneralConfig.Config["Currency"][nameof(TimerAddCurrency)] = value.Ticks.ToString();
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

