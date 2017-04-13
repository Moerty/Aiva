using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using Aiva.Extensions.Giveaway;
using Aiva.Core.Config;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    public class GiveawayViewModel {
        public ICommand StartCommand { get; set; } = new RoutedCommand();
        public ICommand StopCommand { get; set; } = new RoutedCommand();
        public ICommand RollCommand { get; set; } = new RoutedCommand();

        public DispatcherTimer TimerToEnd { get; set; }

        public Giveaway GiveawayInstance { get; set; }
        public Models.GiveawayModel Model { get; set; }

        public GiveawayViewModel() {
            var type = new MahApps.Metro.Controls.MetroContentControl().GetType();

            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(StartCommand, Start));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(StopCommand, Stop));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(RollCommand, Roll));

            // Create Models
            CreateModels();

            TimerToEnd = new DispatcherTimer(DispatcherPriority.Normal) {
                Interval = TimeSpan.FromMinutes(3),
            };
            TimerToEnd.Tick += TimerToEnd_Tick;
        }

        private void TimerToEnd_Tick(object sender, EventArgs e) {
            TimerToEnd.Stop();
            Stop();
        }

        private void Start(object sender, EventArgs e) {

            if (String.IsNullOrEmpty(Model.Options.Keyword)) return;

            var StartModel = new Aiva.Extensions.Models.Giveaway.GiveawayModel {
                Admin = Model.Options.Admin,
                Mod = Model.Options.Mod,
                Sub = Model.Options.Sub,
                User = Model.Options.User,
                SubLuck = Model.Options.SubLuck,
                Keyword = Model.Options.Keyword,
            };

            Model.Winners = new ObservableCollection<string>();
            GiveawayInstance = new Giveaway(StartModel);

            Model.IsStarted = true;
            TimerToEnd.Start();
        }

        private void Roll(object sender, EventArgs e) {
            var raffleWinner = GiveawayInstance.UserList.GetWinner();

            if (Model.UncheckWinner)
                if (Model.Winners.SingleOrDefault(x => String.Compare(raffleWinner.Username, x, StringComparison.OrdinalIgnoreCase) == 0) == null)
                    Model.Winners.Add(raffleWinner.Username);
        }

        private void Stop(object sender = null, EventArgs e = null) {
            Model.IsStarted = false;

            GiveawayInstance.StopGiveawayRegistration();
        }


        private void CreateModels() {
            Model = new Models.GiveawayModel {
                Text = new Models.GiveawayModel.TextModel {
                    StatusActive = LanguageConfig.Instance.GetString("GiveawayStatusActive"),
                    StatusInactive = LanguageConfig.Instance.GetString("GiveawayStatusInactive"),
                    ButtonGiveawayRoll = LanguageConfig.Instance.GetString("GiveawayButtonGiveawayRoll"),
                    ButtonGiveawayStart = LanguageConfig.Instance.GetString("GiveawayButtonGiveawayStart"),
                    ButtonGiveawayStop = LanguageConfig.Instance.GetString("GiveawayButtonGiveawayStop"),
                    SubLuckText = LanguageConfig.Instance.GetString("GiveawaySubLuckText"),
                    CommandWatermark = LanguageConfig.Instance.GetString("GiveawayCommandWatermark"),
                    ExpanderUsersDescription = LanguageConfig.Instance.GetString("GiveawayExpanderUsersDescription"),
                    ExpanderWinnersDescription = LanguageConfig.Instance.GetString("GiveawayExpanderWinnersDescription"),
                    CheckBoxAdmin = LanguageConfig.Instance.GetString("GiveawayCheckBoxAdmin"),
                    CheckBoxMod = LanguageConfig.Instance.GetString("GiveawayCheckBoxMod"),
                    CheckBoxSub = LanguageConfig.Instance.GetString("GiveawayCheckBoxSub"),
                    CheckBoxViewer = LanguageConfig.Instance.GetString("GiveawayCheckBoxViewer"),
                    UncheckWinner = LanguageConfig.Instance.GetString("GiveawayUncheckWinner"),
                }
            };
        }
    }
}