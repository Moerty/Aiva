namespace Aiva.Core.Models {

    using Newtonsoft.Json;

    public class ConfigStorage {

        public partial class Root {

            [JsonProperty("Chat")]
            public Chat Chat { get; set; }

            [JsonProperty("Credentials")]
            public Credentials Credentials { get; set; }

            [JsonProperty("Currency")]
            public Currency Currency { get; set; }

            [JsonProperty("General")]
            public FluffyGeneral General { get; set; }

            [JsonProperty("Interactions")]
            public Interactions Interactions { get; set; }

            [JsonProperty("ModCommands")]
            public ModCommands ModCommands { get; set; }

            [JsonProperty("StreamGames")]
            public StreamGames StreamGames { get; set; }
        }

        public partial class StreamGames {

            [JsonProperty("Bankheist")]
            public Bankheist Bankheist { get; set; }
        }

        public partial class Bankheist {

            [JsonProperty("Cooldowns")]
            public Cooldowns Cooldowns { get; set; }

            [JsonProperty("General")]
            public PurpleGeneral General { get; set; }

            [JsonProperty("Settings")]
            public Settings Settings { get; set; }
        }

        public partial class Settings {

            [JsonProperty("Bank1")]
            public Bank Bank1 { get; set; }

            [JsonProperty("Bank2")]
            public Bank Bank2 { get; set; }

            [JsonProperty("Bank3")]
            public Bank3 Bank3 { get; set; }

            [JsonProperty("Bank4")]
            public Bank Bank4 { get; set; }

            [JsonProperty("Bank5")]
            public Bank Bank5 { get; set; }
        }

        public partial class Bank3 {

            [JsonProperty("MinimumPlayers")]
            public long MinimumPlayers { get; set; }

            [JsonProperty("SuccessRate")]
            public long SuccessRate { get; set; }

            [JsonProperty("WinningMultiplier")]
            public long WinningMultiplier { get; set; }
        }

        public partial class Bank {

            [JsonProperty("MinimumPlayers")]
            public long MinimumPlayers { get; set; }

            [JsonProperty("SuccessRate")]
            public long SuccessRate { get; set; }

            [JsonProperty("WinningMultiplier")]
            public double WinningMultiplier { get; set; }
        }

        public partial class PurpleGeneral {

            [JsonProperty("Active")]
            public bool Active { get; set; }

            [JsonProperty("Command")]
            public string Command { get; set; }
        }

        public partial class Cooldowns {

            [JsonProperty("BankheistCooldown")]
            public long BankheistCooldown { get; set; }

            [JsonProperty("BankheistDuration")]
            public long BankheistDuration { get; set; }
        }

        public partial class ModCommands {

            [JsonProperty("ModCurrency")]
            public ModCurrency ModCurrency { get; set; }
        }

        public partial class ModCurrency {

            [JsonProperty("AddCurrency")]
            public string AddCurrency { get; set; }

            [JsonProperty("RemoveCurrency")]
            public string RemoveCurrency { get; set; }

            [JsonProperty("TransferCurrency")]
            public string TransferCurrency { get; set; }
        }

        public partial class Interactions {

            [JsonProperty("WriteInChatNormalSub")]
            public bool WriteInChatNormalSub { get; set; }

            [JsonProperty("WriteInChatPrimeSub")]
            public bool WriteInChatPrimeSub { get; set; }
        }

        public partial class FluffyGeneral {

            [JsonProperty("BotName")]
            public string BotName { get; set; }

            [JsonProperty("Channel")]
            public string Channel { get; set; }

            [JsonProperty("CommandIdentifier")]
            public string CommandIdentifier { get; set; }
        }

        public partial class Currency {

            [JsonProperty("AddCurrencyFrquently")]
            public bool AddCurrencyFrquently { get; set; }

            [JsonProperty("CurrencyToAddFrequently")]
            public long CurrencyToAddFrequently { get; set; }

            [JsonProperty("TimerAddCurrencyFrequently")]
            public long TimerAddCurrencyFrequently { get; set; }
        }

        public partial class Credentials {

            [JsonProperty("TwitchClientID")]
            public string TwitchClientID { get; set; }

            [JsonProperty("TwitchOAuth")]
            public string TwitchOAuth { get; set; }
        }

        public partial class Chat {

            [JsonProperty("BlacklistWordsChecker")]
            public bool BlacklistWordsChecker { get; set; }

            [JsonProperty("CapsChecker")]
            public bool CapsChecker { get; set; }

            [JsonProperty("LinkChecker")]
            public bool LinkChecker { get; set; }
        }

        public partial class Root {

            public static Root FromJson(string json) => JsonConvert.DeserializeObject<Root>(json, Converter.Settings);
        }

        public class Converter {

            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
            };
        }
    }
}