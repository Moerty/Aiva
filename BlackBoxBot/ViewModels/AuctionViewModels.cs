using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace BlackBoxBot.ViewModels
{
    [PropertyChanged.ImplementPropertyChanged]
    class AuctionViewModels
    {
        public ICommand StopCommand { get; set; } = new RoutedCommand();
        public ICommand StartCommand { get; set; } = new RoutedCommand();
        public Auction.AuctionHandler AuctionHandler { get; set; } = new Auction.AuctionHandler();

        public AuctionViewModels(FrameworkElement control)
        {
            // Initialize
            CreateModels();

            // Commands
            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(StartCommand, StartAuction));
            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(StopCommand, StopAuction));
        }

        private void StartAuction(object sender, EventArgs e)
        {
            if(!AuctionHandler.IsStarted)
            {
                //AuctionHandler.IsStarted = 
                AuctionHandler.Current = new Auction.CurrentAuction(new Auction.Models.InitModel
                {
                    Name = AuctionName,
                    Command = Command,
                    WithTickets = exTickets,
                    WriteStartInChat = WriteInChat,
                    Tickets = Tickets,
                    StartChatMessage = Config.Language.Instance.GetString("AuctionAuctionStart"),
                    StartChatMessageTicket = Config.Language.Instance.GetString("AuctionAuctionStartTicket"),
                });

                AuctionHandler.IsStarted = true;
            }
        }

        private void StopAuction(object sender, EventArgs e)
        {
            AuctionHandler.Current.StopRegistration();
        }

        private void CreateModels()
        {
            Model = new Models.AuctionModel();

            Model.Text = new Models.Text
            {
                NameWatermark = Config.Language.Instance.GetString("AuctionNameWatermark"),
                CommandWatermark = Config.Language.Instance.GetString("AuctionCommandWatermark"),
                TicketExpanderName = Config.Language.Instance.GetString("AuctionTicketExpanderName"),
                ButtonStartName = Config.Language.Instance.GetString("AuctionButtonStartName"),
                ButtonStopName = Config.Language.Instance.GetString("AuctionButtonStopName"),
            };
        }

        public Models.AuctionModel Model { get; set; }
        public string AuctionName { get; set; }
        public string Command { get; set; }
        public bool exTickets { get; set; }
        public int Tickets { get; set; } = 0;
        public bool WriteInChat { get; set; } = true;
    }
}
