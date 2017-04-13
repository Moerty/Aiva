using System;
using System.Windows.Threading;
using System.Windows;
using Aiva.Extensions.Models.Giveaway;
using Aiva.Core.Client;

namespace Aiva.Extensions.Giveaway {
    [PropertyChanged.ImplementPropertyChanged]
    public class Giveaway {
        public GiveawayModel Model { get; set; }
        public bool UncheckWinner { get; set; } = true;
        public int ActiveUsers { get; set; }

        public List UserList { get; set; }

        public DispatcherTimer Timer { get; set; }
        public TimeSpan ActiveTime { get; set; }

        public Giveaway(GiveawayModel Model) {
            this.UserList = new List(Model);

            AivaClient.Client.AivaTwitchClient.OnChatCommandReceived += TwitchClientBBB_OnChatCommandReceived;

            ActiveTime = new TimeSpan(0, 0, 0);
            Timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate {
                ActiveTime += TimeSpan.FromSeconds(1);
            }, Application.Current.Dispatcher);

            Command = Model.Keyword;
        }

        public void StopGiveawayRegistration() {
            AivaClient.Client.AivaTwitchClient.OnChatCommandReceived -= TwitchClientBBB_OnChatCommandReceived;
        }

        // Fix -> Command
        private static string Command;
        private void TwitchClientBBB_OnChatCommandReceived(object sender, TwitchLib.Events.Client.OnChatCommandReceivedArgs e) {
            if (String.Compare(e.Command.Command, Command.Replace("!", ""), StringComparison.OrdinalIgnoreCase) == 0) {
                var result = UserList.AddUserToList(e);

                if (result != ReturnModel.Successfull) return;
            }
        }
    }
}