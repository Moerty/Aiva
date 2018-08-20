using Aiva.Core.Config;
using Aiva.Core.Twitch;
using Aiva.Extensions.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;

namespace Aiva.Extensions.SpamProtection {
    public class Links : IChecker {
        public bool Active {
            get {
                return Config.Instance.Storage.Chat.LinkChecker.Active;
            }
            set {
                Config.Instance.Storage.Chat.LinkChecker.Active = value;
            }
        }

        public int TimeoutTimeInSeconds {
            get {
                return (int)Config.Instance.Storage.Chat.LinkChecker.TimeoutTimeInSeconds;
            }
            set {
                Config.Instance.Storage.Chat.LinkChecker.TimeoutTimeInSeconds = value;
            }
        }

        /// <summary>
        /// Link checker for Viewers and staff
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LinkChecker(object sender, OnMessageReceivedArgs e) {
            if (e.ChatMessage.UserType == UserType.Viewer || e.ChatMessage.UserType == UserType.Staff) {
                var links = e.ChatMessage.Message
                                .Split("\t\n ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                .Where(message
                                    => message.Contains("http://")
                                    || message.Contains("www.")
                                    || message.Contains("https://"));

                if (links.Any()) {
                    AivaClient.Instance.TwitchClient.TimeoutUser(
                        viewer: e.ChatMessage.Username,
                        duration: TimeSpan.FromSeconds(Config.Instance.Storage.Chat.LinkChecker.TimeoutTimeInSeconds),
                        dryRun: AivaClient.DryRun);
                }
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
            Config.Instance.Storage.Chat.LinkChecker.Active = value;
        }

        public void StartListining() {
            AivaClient.Instance.TwitchClient.OnMessageReceived += LinkChecker;
        }

        public void StopListining() {
            AivaClient.Instance.TwitchClient.OnMessageReceived -= LinkChecker;
        }
    }
}