using System;
using System.Collections.ObjectModel;
using Aiva.Extensions.Models.Auction;
using System.Windows;
using System.Windows.Threading;
using Aiva.Core.Client;
using Aiva.Core.Config;
using Aiva.Core.Database;

namespace Aiva.Extensions.Auction {

    /// <summary>
    /// Auction Handler
    /// </summary>
    [PropertyChanged.ImplementPropertyChanged]
    public class AuctionHandler {
        public bool IsStarted { get; set; } = false;
        public CurrentAuction Current { get; set; }
    }



    /// <summary>
    /// The current Auction
    /// </summary>
    [PropertyChanged.ImplementPropertyChanged]
    public class CurrentAuction {
        public ObservableCollection<AddModel> Users { get; set; }
        private readonly InitModel Model;

        public CurrentAuction(InitModel Model) {
            Users = new ObservableCollection<AddModel>();
            this.Model = Model;

            AivaClient.Client.AivaTwitchClient.OnChatCommandReceived += AivaTwitchClient_OnChatCommandReceived;

            if (Model.WriteStartInChat) {
                AivaClient.Client.AivaTwitchClient.SendMessage(FormatStartMessage(Model));

                if (Model.WithTickets) {
                    AivaClient.Client.AivaTwitchClient.SendMessage(FormatTicketMessage(Model));
                }
            }
        }

        /// <summary>
        /// fort start ticket message
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string FormatTicketMessage(InitModel model) {
            var message = LanguageConfig.Instance.GetString("AuctionAuctionStartTicket");

            message = message.Replace("@TICKETCOST@", Model.Tickets.ToString())
                                .Replace("@CURRENCYNAME@", LanguageConfig.Instance.GetString("CurrencyName"));

            return message;
        }

        /// <summary>
        /// Format the start message
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        private static string FormatStartMessage(InitModel Model) {
            var message = LanguageConfig.Instance.GetString("AuctionAuctionStart");

            // Command replace
            message = message.Replace("@COMMAND@",
                        Model.Command.StartsWith("!") ? "\"" + Model.Command + "\"" : "\"" + "!" + Model.Command + "\"");

            return message;
        }

        /// <summary>
        /// if chat command received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AivaTwitchClient_OnChatCommandReceived(object sender, TwitchLib.Events.Client.OnChatCommandReceivedArgs e) {
            // return if user has no valid int
            if (!int.TryParse(e.Command.ArgumentsAsString, out int UserInput)) {
                return;
            }
            if (String.Compare(e.Command.Command, Model.Command, StringComparison.OrdinalIgnoreCase) == 0) {
                if (!Users.Contains(new AddModel { Username = e.Command.ChatMessage.Username })) {

                    var AmountUser = CurrencyHandler.GetCurrency(e.Command.ChatMessage.Username);

                    // With tickets?
                    if (Model.WithTickets) {
                        var ResultTickets = Model.Tickets * UserInput;
                        // Has viewer enough currency?
                        if (AmountUser >= ResultTickets) {
                            CurrencyHandler.RemoveCurrencyAsync(e.Command.ChatMessage.Username, ResultTickets);
                            AddUserToList(e.Command.ChatMessage.Username, ResultTickets);
                        }
                        // with currency
                    } else {
                        CurrencyHandler.RemoveCurrencyAsync(e.Command.ChatMessage.Username, UserInput);
                        AddUserToList(e.Command.ChatMessage.Username, UserInput);
                    }
                }
            }
        }

        /// <summary>
        /// Stop listen to chat command
        /// </summary>
        public void StopRegistration() {
            AivaClient.Client.AivaTwitchClient.OnChatCommandReceived -= AivaTwitchClient_OnChatCommandReceived;
        }

        /// <summary>
        /// Add user to list
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Tickets"></param>
        private void AddUserToList(string Name, int Tickets) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(
                             delegate () {
                                 Users.Add(
                                 new AddModel {
                                     Username = Name,
                                     Tickets = Tickets,
                                 });
                             }));
        }
    }
}
