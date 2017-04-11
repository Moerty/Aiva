using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using AivaBot.Auction.Models;
using TwitchLib.Models.Client;

namespace AivaBot.Auction {
    [PropertyChanged.ImplementPropertyChanged]
    public class AuctionHandler {
        public bool IsStarted { get; set; } = false;
        public CurrentAuction Current { get; set; }
    }

    public class CurrentAuction {
        public ObservableCollection<Models.AddModel> Users { get; set; }
        private Models.InitModel Model;

        public CurrentAuction(Models.InitModel Model) {
            Users = new ObservableCollection<Models.AddModel>();

            this.Model = Model;

            Client.Client.ClientBBB.TwitchClientBBB.OnChatCommandReceived += TwitchClientBBB_OnChatCommandReceived;


            if (Model.WriteStartInChat) {
                Client.Client.ClientBBB.TwitchClientBBB.SendMessage(FormatStartMessage(Model));

                if (Model.WithTickets) {
                    Client.Client.ClientBBB.TwitchClientBBB.SendMessage(FormatTicketMessage(Model));
                }
            }
        }

        private string FormatTicketMessage(InitModel model) {
            string message = Config.Language.Instance.GetString("AuctionAuctionStartTicket");

            message = message.Replace("@TICKETCOST@", Model.Tickets.ToString())
                                .Replace("@CURRENCYNAME@", Config.Language.Instance.GetString("CurrencyName"));

            return message;
        }

        private static string FormatStartMessage(Models.InitModel Model) {
            string message = Config.Language.Instance.GetString("AuctionAuctionStart");

            // Command replace
            message = message.Replace("@COMMAND@",
                        Model.Command.StartsWith("!") ? "\"" + Model.Command + "\"" : "\"" + "!" + Model.Command + "\"");
            return message;
        }

        private void TwitchClientBBB_OnChatCommandReceived(object sender, TwitchLib.Events.Client.OnChatCommandReceivedArgs e) {
            // return if user has no valid int
            int UserInput;
            if (!int.TryParse(e.Command.ArgumentsAsString, out UserInput)) {
                return;
            }
            if (String.Compare(e.Command.Command, Model.Command, StringComparison.OrdinalIgnoreCase) == 0) {
                if (!Users.Contains(new Models.AddModel { Username = e.Command.ChatMessage.Username })) {

                    var AmountUser = Database.CurrencyHandler.GetCurrency(e.Command.ChatMessage.Username);

                    // With tickets?
                    if (Model.WithTickets) {
                        int ResultTickets = Model.Tickets * UserInput;
                        // Has viewer enough currency?
                        if (AmountUser >= ResultTickets) {
                            Database.CurrencyHandler.RemoveCurrencyAsync(e.Command.ChatMessage.Username, ResultTickets);
                            AddUserToList(e.Command.ChatMessage.Username, ResultTickets);
                        }
                        // with currency
                    }
                    else {
                        Database.CurrencyHandler.RemoveCurrencyAsync(e.Command.ChatMessage.Username, UserInput);
                        AddUserToList(e.Command.ChatMessage.Username, UserInput);
                    }
                }
            }
        }

        public void StopRegistration() {
            Client.Client.ClientBBB.TwitchClientBBB.OnChatCommandReceived -= TwitchClientBBB_OnChatCommandReceived;
        }

        private void AddUserToList(string Name, int Tickets) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(
                             delegate () {
                                 Users.Add(
                                 new Models.AddModel {
                                     Username = Name,
                                     Tickets = Tickets,
                                 });
                             }));
        }
    }
}
