namespace Aiva.Core.Config {
    using Newtonsoft.Json;

    public static class Storage {
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
            public Bank Bank3 { get; set; }

            [JsonProperty("Bank4")]
            public Bank Bank4 { get; set; }

            [JsonProperty("Bank5")]
            public Bank Bank5 { get; set; }
        }

        public partial class Bank {
            [JsonProperty("MinimumPlayers")]
            public int MinimumPlayers { get; set; }

            [JsonProperty("SuccessRate")]
            public int SuccessRate { get; set; }

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

            [JsonProperty("BotUserID")]
            public string BotUserID { get; set; }
        }

        public partial class Currency {
            [JsonProperty("AddCurrencyFrquently")]
            public bool AddCurrencyFrquently { get; set; }

            [JsonProperty("CurrencyToAddFrequently")]
            public long CurrencyToAddFrequently { get; set; }

            [JsonProperty("TimerAddCurrencyFrequently")]
            public long TimerAddCurrencyFrequently { get; set; }

            [JsonProperty("CurrencyCommands")]
            public CurrencyCommands CurrencyCommands { get; set; }
        }

        public partial class CurrencyCommands {
            [JsonProperty("GetCurrency")]
            public string GetCurrency { get; set; }
        }

        public partial class Credentials {
            [JsonProperty("TwitchClientID")]
            public string TwitchClientID { get; set; }

            [JsonProperty("TwitchOAuth")]
            public string TwitchOAuth { get; set; }
        }

        public partial class Chat {
            [JsonProperty("BlacklistWordsChecker")]
            public Checker BlacklistWordsChecker { get; set; }

            [JsonProperty("CapsChecker")]
            public CapsChecker CapsChecker { get; set; }

            [JsonProperty("LinkChecker")]
            public Checker LinkChecker { get; set; }

            [JsonProperty("SpamChecker")]
            public Checker SpamChecker { get; set; }
        }

        public partial class Checker {
            [JsonProperty("Active")]
            public bool Active { get; set; }

            [JsonProperty("TimeoutTimeInSeconds")]
            public long TimeoutTimeInSeconds { get; set; }
        }

        public partial class CapsChecker {
            [JsonProperty("Active")]
            public bool Active { get; set; }

            [JsonProperty("TimeoutTimeInSeconds")]
            public long TimeoutTimeInSeconds { get; set; }

            [JsonProperty("MaxUpperCharactersInMessage")]
            public long MaxUpperCharactersInMessage { get; set; }

            [JsonProperty("MaxUpperCharactersConsecurivelyInMessage")]
            public long MaxUpperCharactersConsecurivelyInMessage { get; set; }

            [JsonProperty("MaxUpperCharactersConsecurivelyActive")]
            public bool MaxUpperCharactersConsecurivelyActive { get; set; }

            [JsonProperty("MoreUpperThanLowerCharactersCheck")]
            public bool MoreUpperThanLowerCharactersCheck { get; set; }

            [JsonProperty("MaxCharactersInAWordActive")]
            public bool MaxCharactersInAWordActive { get; set; }

            [JsonProperty("MaxCharactersInAWord")]
            public long MaxCharactersInAWord { get; set; }
        }

        /// <summary>
        /// Serialize class to File
        /// </summary>
        public partial class Root {
            public static Root FromJson(string json) => JsonConvert.DeserializeObject<Root>(json, Converter.Settings);
        }

        public static class Converter {
            public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
            };
        }
    }
}