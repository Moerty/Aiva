using Aiva.Core.Config;
using Aiva.Core.Database.Storage;
using Aiva.Core.Twitch;
using Aiva.Extensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;

namespace Aiva.Extensions.SpamProtection {
    public class Blacklist : IChecker {
        public int TimeoutTime {
            get {
                return (int)Config.Instance.Storage.Chat.BlacklistWordsChecker.TimeoutTimeInSeconds;
            }
            set {
                Config.Instance.Storage.Chat.BlacklistWordsChecker.TimeoutTimeInSeconds = value;
            }
        }

        public bool Active {
            get {
                return Config.Instance.Storage.Chat.BlacklistWordsChecker.Active;
            }
            set {
                Config.Instance.Storage.Chat.BlacklistWordsChecker.Active = value;
            }
        }

        private readonly Core.Database.Handlers.SpamProtection.BlacklistHandler _databaseBlacklistHandler;

        public Blacklist() {
            _databaseBlacklistHandler = new Core.Database.Handlers.SpamProtection.BlacklistHandler();
        }

        public bool IsWordInList(string word, bool caseSensetive) {
            return _databaseBlacklistHandler.IsWordInList(word, caseSensetive);
        }

        /// <summary>
        /// Get words from database
        /// </summary>
        /// <returns></returns>
        public List<BlacklistedWords> GetWords() {
            return _databaseBlacklistHandler.GetBlacklistedWords();
        }

        public void AddWordToBlacklist(string wordToAdd) {
            _databaseBlacklistHandler.AddWord(wordToAdd);
        }

        public void RemoveWord(string selectedWord) {
            _databaseBlacklistHandler.RemoveWord(selectedWord);
        }

        /// <summary>
        /// Check if User have written a BlacklistedWord and timeout him
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BlacklistWordsChecker(object sender, OnMessageReceivedArgs e) {
            var result = _databaseBlacklistHandler.IsBlacklistedWordInMessage(e.ChatMessage.Message);

            if (result) {
                AivaClient.Instance.TwitchClient.TimeoutUser(
                    viewer: e.ChatMessage.Username,
                    duration: TimeSpan.FromSeconds(Config.Instance.Storage.Chat.BlacklistWordsChecker.TimeoutTimeInSeconds),
                    dryRun: AivaClient.DryRun);
            }
        }

        public static void OpenWikiInBrowser() {
            Process.Start(new ProcessStartInfo("https://aiva.it0.me/chat/Checkers"));
        }

        /// <summary>
        /// Change active status
        /// </summary>
        /// <param name="value"></param>
        public void ChangeStatus(bool value) {
            Config.Instance.Storage.Chat.BlacklistWordsChecker.Active = value;
        }

        public void StartListining() {
            AivaClient.Instance.TwitchClient.OnMessageReceived += BlacklistWordsChecker;
        }

        public void StopListining() {
            AivaClient.Instance.TwitchClient.OnMessageReceived += BlacklistWordsChecker;
        }
    }
}