using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace AivaBot.Auction
{
    [PropertyChanged.ImplementPropertyChanged]
    public class AuctionHandler
    {
        public bool IsStarted { get; set; } = false;
        public CurrentAuction Current { get; set; }
    }

    public class CurrentAuction
    {
        public ObservableCollection<Models.AddModel> Users { get; set; }

        private Models.InitModel Model;

        public CurrentAuction(Models.InitModel Model)
        {
            Users = new ObservableCollection<Models.AddModel>();

            this.Model = Model;

            Client.Client.ClientBBB.TwitchClientBBB.OnChatCommandReceived += TwitchClientBBB_OnChatCommandReceived;


            if (Model.WriteStartInChat)
                Client.Client.ClientBBB.TwitchClientBBB.SendMessage(Model.StartChatMessage.Replace("@COMMAND@",Model.Command));
        }

        private void TwitchClientBBB_OnChatCommandReceived(object sender, TwitchLib.Events.Client.OnChatCommandReceivedArgs e)
        {
            if (String.Compare(e.Command.Command, Model.Command, StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!Users.Contains(new Models.AddModel { Username = e.Command.ChatMessage.Username }))
                {
                    int result;
                    if (int.TryParse(e.Command.ArgumentsAsString, out result))
                    {
                        if (Model.WithTickets == false)
                        {
                            AddUserToList(e.Command.ChatMessage.Username, result);
                        }
                        else
                        {
                            var AmountUser = Database.CurrencyHandler.GetCurrency(e.Command.ChatMessage.Username);

                            result = Model.Tickets * result;
                            if (AmountUser >= result)
                            {
                                Database.CurrencyHandler.RemoveCurrencyAsync(e.Command.ChatMessage.Username, result);
                                AddUserToList(e.Command.ChatMessage.Username, result);
                            }
                        }
                    }
                }
            }
        }

        public void StopRegistration()
        {
            Client.Client.ClientBBB.TwitchClientBBB.OnChatCommandReceived -= TwitchClientBBB_OnChatCommandReceived;
        }


        private void AddUserToList(string Name, int Tickets)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(
                             delegate ()
                             {
                                 Users.Add(
                                 new Models.AddModel
                                 {
                                     Username = Name,
                                     Tickets = Tickets,
                                 });
                             })); 

            /*Users.Add(
                new Models.AddModel
                {
                    Username = Name,
                    Tickets = Tickets,
                }); */
        }
    }
}
