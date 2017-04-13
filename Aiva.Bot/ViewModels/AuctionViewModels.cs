using System;
using System.Windows;
using System.Windows.Input;
using Aiva.Extensions.Auction;
using Aiva.Extensions.Models.Auction;
using Aiva.Core.Config;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    class AuctionViewModels {
        public ICommand StopCommand { get; set; } = new RoutedCommand();
        public ICommand StartCommand { get; set; } = new RoutedCommand();
        public AuctionHandler AuctionHandler { get; set; } = new AuctionHandler();

        public AuctionViewModels(FrameworkElement control) {
            // Initialize
            CreateModels();

            // Commands
            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(StartCommand, StartAuction));
            CommandManager.RegisterClassCommandBinding(control.GetType(), new CommandBinding(StopCommand, StopAuction));
        }

        private void StartAuction(object sender, EventArgs e) {
            if (!AuctionHandler.IsStarted) {
                AuctionHandler.Current = new CurrentAuction(new InitModel {
                    Name = AuctionName,
                    Command = Command,
                    WithTickets = ExTickets,
                    WriteStartInChat = WriteInChat,
                    Tickets = Tickets
                });

                AuctionHandler.IsStarted = true;
            }
        }

        private void StopAuction(object sender, EventArgs e) {
            AuctionHandler.Current.StopRegistration();
            AuctionHandler.IsStarted = false;
        }

        private void CreateModels() {
            Model = new Models.AuctionModel {
                Text = new Models.Text {
                    NameWatermark = LanguageConfig.Instance.GetString("AuctionNameWatermark"),
                    CommandWatermark = LanguageConfig.Instance.GetString("AuctionCommandWatermark"),
                    TicketExpanderName = LanguageConfig.Instance.GetString("AuctionTicketExpanderName"),
                    ButtonStartName = LanguageConfig.Instance.GetString("AuctionButtonStartName"),
                    ButtonStopName = LanguageConfig.Instance.GetString("AuctionButtonStopName"),
                    AuctionExpanderSettingsText = LanguageConfig.Instance.GetString("AuctionExpanderSettingsText"),
                    AuctionWriteInChatText = LanguageConfig.Instance.GetString("AuctionWriteInChatText"),
                    AuctionHeaderTicketsText = LanguageConfig.Instance.GetString("AuctionHeaderTicketsText"),
                    AuctionHeaderUsernameText = LanguageConfig.Instance.GetString("AuctionHeaderUsernameText"),
                }
            };
        }

        public Models.AuctionModel Model { get; set; }
        public string AuctionName { get; set; }
        public string Command { get; set; }
        public bool ExTickets { get; set; }
        public int Tickets { get; set; } = 1;
        public bool WriteInChat { get; set; } = true;
    }
}
