using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveCharts.Wpf;
using Aiva.Extensions.Models.Bets;
using Aiva.Core.Client;

namespace AivaBot.Bets {
    [PropertyChanged.ImplementPropertyChanged]
    public class BetsHandler {
        public bool IsStarted { get; set; }
        public CurrentBet Current { get; set; }

        public void StopBet() {
            IsStarted = false;
        }
    }

    [PropertyChanged.ImplementPropertyChanged]
    public class CurrentBet {
        public ObservableCollection<UserModel> Users { get; set; }
        public SeriesCollection SeriesCollection { get; set; }
        public ObservableValue BetValue1 { get; set; } = new ObservableValue(00.00);
        public ObservableValue BetValue2 { get; set; } = new ObservableValue(00.00);
        public ObservableValue BetterCount1 { get; set; } = new ObservableValue(00.00);
        public ObservableValue BetterCount2 { get; set; } = new ObservableValue(00.00);
        public bool ShowChart { get; set; } = false;

        private readonly System.Timers.Timer TimerToEndRegistration;
        private string Command { get; set; }

        public CurrentBet(TimeSpan TimeToEnd, string Command) {
            Users = new ObservableCollection<UserModel>();
            this.Command = Command;

            AivaClient.Client.AivaTwitchClient.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;

            // Timer
            TimerToEndRegistration = new System.Timers.Timer();
            TimerToEndRegistration.Elapsed += EndRegistration;
            TimerToEndRegistration.Interval = TimeToEnd.TotalMilliseconds;
            TimerToEndRegistration.Start();
        }

        public void EndRegistration(object sender = null, EventArgs e = null) {
            TimerToEndRegistration.Stop();
            AivaClient.Client.AivaTwitchClient.OnChatCommandReceived -= TwitchClient_OnChatCommandReceived;
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
            if (!int.TryParse(args[1], out int Bet)) return false;

            var user = Users.SingleOrDefault(x => x.Name == name);

            if (user == null) {
                if (args[0].Length == 1) {
                    System.Windows.Application.Current.Dispatcher.Invoke((Action)delegate {
                        Users.Add(
                        new UserModel {
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

            var List = new List<Aiva.Core.Models.Database.CurrencyHandlerModels.CurrencyAddList>();

            foreach (var user in Users) {
                switch (Team) {
                    case 'a': {
                            List.Add(new Aiva.Core.Models.Database.CurrencyHandlerModels.CurrencyAddList {
                                Name = user.Name,
                                Value = user.Value + (user.Value * (multiplicator / 100)),
                            });
                        }
                        break;
                    case 'b': {
                            List.Add(new Aiva.Core.Models.Database.CurrencyHandlerModels.CurrencyAddList {
                                Name = user.Name,
                                Value = user.Value + (user.Value * (multiplicator / 100)),
                            });
                        }
                        break;
                    default: {
                            Console.WriteLine("ERROR");
                        }
                        break;
                }
            }

            Aiva.Core.Database.CurrencyHandler.UpdateCurrencyListAsync(List);
        }
    }

    public static class Extensions {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
            foreach (var cur in enumerable) {
                action?.Invoke(cur);
            }
        }
    }
}
