using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace AivaBot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    public class GiveawayViewModel {
        public ICommand StartCommand { get; set; } = new RoutedCommand();
        public ICommand StopCommand { get; set; } = new RoutedCommand();
        public ICommand RollCommand { get; set; } = new RoutedCommand();

        public DispatcherTimer TimerToEnd { get; set; }

        public Giveaway.Giveaway GiveawayInstance { get; set; }
        public Models.GiveawayModel Model { get; set; }

        public GiveawayViewModel() {
            var type = new MahApps.Metro.Controls.MetroContentControl().GetType();

            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(StartCommand, Start));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(StopCommand, Stop));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(RollCommand, Roll));

            // Create Models
            CreateModels();

            TimerToEnd = new DispatcherTimer(DispatcherPriority.Normal);
            TimerToEnd.Interval = TimeSpan.FromMinutes(3);
            TimerToEnd.Tick += TimerToEnd_Tick;
        }

        private void TimerToEnd_Tick(object sender, EventArgs e) {
            TimerToEnd.Stop();
            Stop();
        }

        private void Start(object sender, EventArgs e) {

            if (String.IsNullOrEmpty(Model.Options.Keyword)) return;

            var StartModel = new Giveaway.Models.GiveawayModel {
                Admin = Model.Options.Admin,
                Mod = Model.Options.Mod,
                Sub = Model.Options.Sub,
                User = Model.Options.User,
                SubLuck = Model.Options.SubLuck,
                Keyword = Model.Options.Keyword,
            };

            Model.Winners = new ObservableCollection<string>();
            GiveawayInstance = new Giveaway.Giveaway(StartModel);

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
            Model = new Models.GiveawayModel();

            Model.Text = new Models.GiveawayModel.TextModel {
                StatusActive = Config.Language.Instance.GetString("GiveawayStatusActive"),
                StatusInactive = Config.Language.Instance.GetString("GiveawayStatusInactive"),
                ButtonGiveawayRoll = Config.Language.Instance.GetString("GiveawayButtonGiveawayRoll"),
                ButtonGiveawayStart = Config.Language.Instance.GetString("GiveawayButtonGiveawayStart"),
                ButtonGiveawayStop = Config.Language.Instance.GetString("GiveawayButtonGiveawayStop"),
                SubLuckText = Config.Language.Instance.GetString("GiveawaySubLuckText"),
                CommandWatermark = Config.Language.Instance.GetString("GiveawayCommandWatermark"),
                ExpanderUsersDescription = Config.Language.Instance.GetString("GiveawayExpanderUsersDescription"),
                ExpanderWinnersDescription = Config.Language.Instance.GetString("GiveawayExpanderWinnersDescription"),
                CheckBoxAdmin = Config.Language.Instance.GetString("GiveawayCheckBoxAdmin"),
                CheckBoxMod = Config.Language.Instance.GetString("GiveawayCheckBoxMod"),
                CheckBoxSub = Config.Language.Instance.GetString("GiveawayCheckBoxSub"),
                CheckBoxViewer = Config.Language.Instance.GetString("GiveawayCheckBoxViewer"),
                UncheckWinner = Config.Language.Instance.GetString("GiveawayUncheckWinner"),
            };
        }
    }
}