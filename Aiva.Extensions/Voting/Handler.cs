using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Events.Client;

namespace Aiva.Extensions.Voting {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Handler {
        public bool IsStarted { get; set; }
        public Models.Voting.Properties Properties { get; set; }
        public Models.Voting.ChartValues ChartValues { get; set; }

        public void StopRegistration() {
            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived -= ChatCommandReceived;
        }

        private void ChatCommandReceived(object sender, OnChatCommandReceivedArgs e) {
            var command = Properties.Command.TrimStart('!');

            if(string.Compare(command, e.Command.CommandText, true) == 0) {
                if(String.Compare(e.Command.ArgumentsAsString, Properties.Options.Option1.Option, true) == 0) {
                    ChartValues.Option1.Value++;
                    ChartValues.Option1Usernames.Add(e.Command.ChatMessage.DisplayName);
                } else if (String.Compare(e.Command.ArgumentsAsString, Properties.Options.Option2.Option, true) == 0) {
                    ChartValues.Option2.Value++;
                    ChartValues.Option2Usernames.Add(e.Command.ChatMessage.DisplayName);
                } else if (String.Compare(e.Command.ArgumentsAsString, Properties.Options.Option3.Option, true) == 0) {
                    ChartValues.Option3.Value++;
                    ChartValues.Option3Usernames.Add(e.Command.ChatMessage.DisplayName);
                } else if (String.Compare(e.Command.ArgumentsAsString, Properties.Options.Option4.Option, true) == 0) {
                    ChartValues.Option4.Value++;
                    ChartValues.Option4Usernames.Add(e.Command.ChatMessage.DisplayName);
                } else if (String.Compare(e.Command.ArgumentsAsString, Properties.Options.Option5.Option, true) == 0) {
                    ChartValues.Option5.Value++;
                    ChartValues.Option5Usernames.Add(e.Command.ChatMessage.DisplayName);
                } else if (String.Compare(e.Command.ArgumentsAsString, Properties.Options.Option6.Option, true) == 0) {
                    ChartValues.Option6.Value++;
                    ChartValues.Option6Usernames.Add(e.Command.ChatMessage.DisplayName);
                }
            }
        }

        public void StartRegistration() {
            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived += ChatCommandReceived;
        }

        public void StopVoting() {
            StopRegistration();
            IsStarted = false;
        }
    }
}
