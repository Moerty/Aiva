using Aiva.Core.Config;
using Aiva.Core.Twitch;
using Aiva.Extensions.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using TwitchLib.Enums;
using TwitchLib.Events.Client;
using TwitchLib.Extensions.Client;

namespace Aiva.Extensions.SpamProtection {
    public class Caps : IChecker {
        public bool Active {
            get {
                return Config.Instance.Storage.Chat.CapsChecker.Active;
            }
            set {
                Config.Instance.Storage.Chat.CapsChecker.Active = value;
            }
        }

        public int TimeoutTimeInSeconds {
            get {
                return (int)Config.Instance.Storage.Chat.CapsChecker.TimeoutTimeInSeconds;
            }
            set {
                Config.Instance.Storage.Chat.CapsChecker.TimeoutTimeInSeconds = value;
            }
        }

        public bool MaxUpperCharactersConsecurivelyActive {
            get {
                return Config.Instance.Storage.Chat.CapsChecker.MaxUpperCharactersConsecurivelyActive;
            }
            set {
                Config.Instance.Storage.Chat.CapsChecker.MaxUpperCharactersConsecurivelyActive = value;
            }
        }

        public int MaxUpperCharactersConsecurivelyInMessage {
            get {
                return (int)Config.Instance.Storage.Chat.CapsChecker.MaxUpperCharactersConsecurivelyInMessage;
            }
            set {
                Config.Instance.Storage.Chat.CapsChecker.MaxUpperCharactersConsecurivelyInMessage = value;
            }
        }

        public bool MaxCharactersInAWordActive {
            get {
                return Config.Instance.Storage.Chat.CapsChecker.MaxCharactersInAWordActive;
            }
            set {
                Config.Instance.Storage.Chat.CapsChecker.MaxCharactersInAWordActive = value;
            }
        }

        public bool MoreUpperThanLowerCharactersCheck {
            get {
                return Config.Instance.Storage.Chat.CapsChecker.MoreUpperThanLowerCharactersCheck;
            }
            set {
                Config.Instance.Storage.Chat.CapsChecker.MoreUpperThanLowerCharactersCheck = value;
            }
        }

        public int MaxCharactersInAWord {
            get {
                return (int)Config.Instance.Storage.Chat.CapsChecker.MaxCharactersInAWord;
            }
            set {
                Config.Instance.Storage.Chat.CapsChecker.MaxCharactersInAWord = value;
            }
        }

        /// <summary>
        /// Caps checker for Viewers and stuff
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CapsChecker(object sender, OnMessageReceivedArgs e) {
            if (Config.Instance.Storage.Chat.CapsChecker.MaxUpperCharactersConsecurivelyActive) {
                CheckCharactersConsecutively(e);
            }

            if (Config.Instance.Storage.Chat.CapsChecker.MoreUpperThanLowerCharactersCheck) {
                CheckForMoreUpperThanLowerCharacters(e);
            }
        }

        /// <summary>
        /// Checks string for more upper chars than lower chars
        /// </summary>
        /// <param name="e"></param>
        private void CheckForMoreUpperThanLowerCharacters(OnMessageReceivedArgs e) {
            var upperChars = e.ChatMessage.Message.Count(c => Char.IsUpper(c));
            var lowerChars = e.ChatMessage.Message.Count(c => Char.IsLower(c));

            if (upperChars >= lowerChars) {
                AivaClient.Instance.TwitchClient.TimeoutUser(
                        viewer: e.ChatMessage.Username,
                        duration: TimeSpan.FromSeconds(Config.Instance.Storage.Chat.CapsChecker.TimeoutTimeInSeconds),
                        dryRun: AivaClient.DryRun);
            }
        }

        /// <summary>
        /// Checks for more than x chars in a row
        /// </summary>
        /// <param name="e"></param>
        private void CheckCharactersConsecutively(OnMessageReceivedArgs e) {
            if (e.ChatMessage.UserType == UserType.Viewer || e.ChatMessage.UserType == UserType.Staff) {
                if (CheckForCaps(e.ChatMessage.Message)) {
                    AivaClient.Instance.TwitchClient.TimeoutUser(
                        viewer: e.ChatMessage.Username,
                        duration: TimeSpan.FromSeconds(Config.Instance.Storage.Chat.CapsChecker.TimeoutTimeInSeconds),
                        dryRun: AivaClient.DryRun);
                }
            }
        }

        /// <summary>
        /// Open wiki in browser
        /// </summary>
        public static void OpenWikiInBrowser() {
            Process.Start(new ProcessStartInfo("https://aiva.it0.me/chat/Checkers"));
        }

        /// <summary>
        /// Check string for upper chars
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool CheckForCaps(string input) {
            int result = 0;

            foreach (char c in input) {
                if (char.IsUpper(c)) {
                    result++;

                    if (result >= Config.Instance.Storage.Chat.CapsChecker.MaxUpperCharactersInMessage) {
                        return true;
                    }
                } else {
                    result = 0;
                }
            }

            return false;
        }

        /// <summary>
        /// Change active status
        /// </summary>
        /// <param name="value"></param>
        public void ChangeStatus(bool value) {
            Config.Instance.Storage.Chat.CapsChecker.Active = value;
        }

        public void StartListining() {
            AivaClient.Instance.TwitchClient.OnMessageReceived += CapsChecker;
        }

        public void StopListining() {
            AivaClient.Instance.TwitchClient.OnMessageReceived -= CapsChecker;
        }
    }
}