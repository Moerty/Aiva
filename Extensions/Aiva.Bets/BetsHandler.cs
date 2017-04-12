using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveCharts.Wpf;
using System.Windows.Threading;

namespace AivaBot.Bets {
    [PropertyChanged.ImplementPropertyChanged]
    public class BetsHandler {
        public bool IsStarted { get; set; } = false;
        public CurrentBet Current { get; set; }

        public void StopBet() {
            IsStarted = false;
        }
    }

    [PropertyChanged.ImplementPropertyChanged]
    public class CurrentBet {
        public ObservableCollection<Models.UserModel> Users { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public ObservableValue BetValue1 { get; set; } = new ObservableValue(00.00);
        public ObservableValue BetValue2 { get; set; } = new ObservableValue(00.00);
        public ObservableValue BetterCount1 { get; set; } = new ObservableValue(00.00);
        public ObservableValue BetterCount2 { get; set; } = new ObservableValue(00.00);
        public bool ShowChart { get; set; } = false;

        private DispatcherTimer TimerToEndRegistration;
        private string Command { get; set; }

        public CurrentBet(int MinutesTimer, string Command) {
            Users = new ObservableCollection<Models.UserModel>();
            this.Command = Command;

            Client.Client.ClientBBB.TwitchClientBBB.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;

            // Timer
            TimerToEndRegistration = new DispatcherTimer(DispatcherPriority.Normal);
            TimerToEndRegistration.Tick += EndRegistration;
        }

        public void EndRegistration(object sender = null, EventArgs e = null) {
            Client.Client.ClientBBB.TwitchClientBBB.OnChatCommandReceived -= TwitchClient_OnChatCommandReceived;
        }

        private void TwitchClient_OnChatCommandReceived(object sender, TwitchLib.Events.Client.OnChatCommandReceivedArgs e) {
            if (String.Compare(Command, e.Command.Command, StringComparison.OrdinalIgnoreCase) == 0)
                AddUser(e.Command.ChatMessage.Username, e.Command.ArgumentsAsList);
        }

        /// <summary>
        /// Add User to List
        /// </summary>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public bool AddUser(string name, List<string> args) {
            // if (args.Count != 2) throw new Exceptions.ExceptionParametersNotValid("not 2 parameters!");
            int Bet;
            if (int.TryParse(args[1], out Bet) == false) return false;


            //Models.UsersModel user = Users.SingleOrDefault(x => x.Name == name);
            var user = "asd";
            if (user == null || true) {
                if (args[0].Length == 1) {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                        Users.Add(
                        new Models.UserModel {
                            Name = name,
                            Team = Convert.ToChar(args[0].ToUpper()),
                            Value = Bet,
                        });

                        if (SeriesCollection == null) {
                            SeriesCollection = new SeriesCollection
                            {
                                new StackedRowSeries
                                {
                                    Title = "Team 1",
                                    Values = new ChartValues<ObservableValue> { BetterCount1, BetValue1 },
                                    StackMode = StackMode.Percentage,
                                    DataLabels = true,
                                    LabelPoint = p => p.X.ToString(),
                                },
                                new StackedRowSeries
                                {
                                    Title = "Team 2",
                                    Values = new ChartValues<ObservableValue> { BetterCount2, BetValue2 },
                                    StackMode = StackMode.Percentage,
                                    DataLabels = true,
                                    LabelPoint = p => p.X.ToString(),
                                }
                            };
                        }
                    });

                    switch (Convert.ToChar(args[0])) {
                        case 'a': {
                                BetterCount1.Value++;
                                BetValue1.Value += Bet;
                                break;
                            }
                        case 'b': {
                                BetterCount2.Value++;
                                BetValue2.Value += Bet;
                                break;
                            }
                    }

                    ShowChart = true;

                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove User from list
        /// </summary>
        /// <param name="name"></param>
        public void RemoveUser(string name) {
            var user = Users.SingleOrDefault(x => x.Name == name);

            if (user != null)
                Users.Remove(user);
        }

        public void PayOut(char team, int multiplicator) {
            Task.Run(() => PayOutTask(team, multiplicator));
        }

        private void PayOutTask(char Team, int multiplicator) {
            if (!Users.Any()) return;

            List<Database.Models.CurrencyHandlerModels.CurrencyAddList> List = new List<Database.Models.CurrencyHandlerModels.CurrencyAddList>();

            foreach (var user in Users) {
                switch (Team) {
                    case 'a': {
                            Users.ForEach(x => {
                                List.Add(new Database.Models.CurrencyHandlerModels.CurrencyAddList {
                                    Name = user.Name,
                                    Value = user.Value + (user.Value * (multiplicator / 100)),
                                });
                            });
                        }
                        break;
                    case 'b': {
                            Users.ForEach(x => {
                                List.Add(new Database.Models.CurrencyHandlerModels.CurrencyAddList {
                                    Name = user.Name,
                                    Value = user.Value + (user.Value * (multiplicator / 100)),
                                });
                            });
                        }
                        break;
                    default: {
                            Console.WriteLine("ERROR");
                        }
                        break;
                }
            }

            Database.CurrencyHandler.UpdateCurrencyListAsync(List);
        }
    }

    public static class Extensions {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
            foreach (var cur in enumerable) {
                action(cur);
            }
        }
    }
}
