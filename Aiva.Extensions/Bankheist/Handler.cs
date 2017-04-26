using Aiva.Core;
using Aiva.Core.Config;
using System;
using TwitchLib.Events.Client;
using Aiva.Extensions.Models;

namespace Aiva.Extensions.Bankheist {
    public class Handler {
        public Bankheist CurrentBankheist { get; private set; }
        public Models.Bankheist.BankheistInitModel InitModel { get; private set; }
        public System.Timers.Timer BankheistEndTimer { get; private set; }
        public System.Timers.Timer NewBankheistTimer { get; private set; }

        private bool IsOnCooldown = false;

        public Handler(Models.Bankheist.BankheistInitModel Model) {
            this.InitModel = Model;
            SetupTimers();

            if (Convert.ToBoolean(Config.Instance["Bankheist"]["Active"]))
                StartListining();
        }

        /// <summary>
        /// Setup the Bankheist Timers
        /// </summary>
        private void SetupTimers() {
            BankheistEndTimer = new System.Timers.Timer {
                AutoReset = false,
                Interval = TimeSpan.Parse(Config.Instance["Bankheist"]["BankheistDuration"]).TotalMilliseconds,
            };
            BankheistEndTimer.Elapsed += BankheistEndTimer_Elapsed;


            NewBankheistTimer = new System.Timers.Timer {
                AutoReset = false,
                Interval = TimeSpan.Parse(Config.Instance["Bankheist"]["BankheistCooldown"]).TotalMilliseconds,
            };
            NewBankheistTimer.Elapsed += NewBankheistTimer_Elapsed;
        }

        /// <summary>
        /// Write Bankheist start in Chat
        /// </summary>
        private void WriteStartInChat() {
            // Write in Chat
            AivaClient.Instance.AivaTwitchClient.SendMessage(
                Text.Instance.GetString("BankheistStartText")
                .Replace("@COMMAND@", InitModel.Command));
        }

        /// <summary>
        /// Bankheist Cooldown over
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewBankheistTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            IsOnCooldown = false;
            CurrentBankheist = null;
        }

        /// <summary>
        /// End Bankheist Timer tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BankheistEndTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            StopListining();
            NewBankheistTimer.Start();
            CurrentBankheist.PayOut();
            IsOnCooldown = true;
        }

        /// <summary>
        /// Start listening to TwitchClient
        /// </summary>
        public void StartListining() {
            // listen to Commands
            AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived += OnChatCommandReceived;
        }

        /// <summary>
        /// Stop listening to TwitchClient
        /// </summary>
        public void StopListining() {
            AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived -= OnChatCommandReceived;
        }

        /// <summary>
        /// Chat command received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e) {
            if ((String.Compare(e.Command.Command, Config.Instance["Bankheist"]["Command"])) == 0) {
                if (!IsOnCooldown) {
                    if (CurrentBankheist == null) {
                        CurrentBankheist = new Bankheist(InitModel);

                        // write start in chat
                        WriteStartInChat();

                        // start bankheist end timer
                        BankheistEndTimer.Start();
                    }

                    CurrentBankheist.AddUserToBankheist(e.Command.ChatMessage.Username,
                        Convert.ToInt64(e.Command.ChatMessage.UserId), e.Command.ArgumentsAsString);

                } else {
                    AivaClient.Instance.AivaTwitchClient.SendMessage("Bankheist is on Cooldown!");
                }
            }
        }
    }
}
