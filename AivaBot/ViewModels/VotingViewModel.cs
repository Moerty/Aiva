using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using static AivaBot.ViewModels.Extension;

namespace AivaBot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    class VotingViewModel {
        public bool IsStarted { get; set; } = false;
        public bool ChartPie { get; set; } = true;
        public bool ChartBasic { get; set; } = false;
        public bool ChartDonut { get; set; } = false;

        public string Command { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }
        public string Option6 { get; set; }
        public string Option7 { get; set; }
        public string Option8 { get; set; }

        public ObservableValue CountOption1 { get; set; } = new ObservableValue(0);
        public ObservableValue CountOption2 { get; set; } = new ObservableValue(0);
        public ObservableValue CountOption3 { get; set; } = new ObservableValue(0);
        public ObservableValue CountOption4 { get; set; } = new ObservableValue(0);
        public ObservableValue CountOption5 { get; set; } = new ObservableValue(0);
        public ObservableValue CountOption6 { get; set; } = new ObservableValue(0);
        public ObservableValue CountOption7 { get; set; } = new ObservableValue(0);
        public ObservableValue CountOption8 { get; set; } = new ObservableValue(0);

        public string Name { get; set; }


        public ICommand StartVoting { get; set; } = new RoutedCommand();
        public ICommand StopVoting { get; set; } = new RoutedCommand();

        public SeriesCollection tChartCollection { get; set; }
        public SeriesCollection dChartCollection { get; set; }
        public SeriesCollection bChartCollection { get; set; }
        public string[] OptionNames { get; set; }

        public VotingViewModel() {
            CommandManager.RegisterClassCommandBinding(new MahApps.Metro.Controls.MetroContentControl().GetType(), new CommandBinding(StartVoting, startVoting));
            CommandManager.RegisterClassCommandBinding(new MahApps.Metro.Controls.MetroContentControl().GetType(), new CommandBinding(StopVoting, stopVoting));

            tChartCollection = new SeriesCollection();
            dChartCollection = new SeriesCollection();
            bChartCollection = new SeriesCollection();
        }

        public Func<ChartPoint, string> PointLabel { get; set; }


        private void startVoting(Object sender, EventArgs e) {
            if (String.IsNullOrEmpty(Command)) return;
            tChartCollection = new SeriesCollection();
            DrawTChart();
            DrawDChart();
            DrawBChart();
            IsStarted = true;
            Client.Client.ClientBBB.TwitchClientBBB.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;
        }


        List<string> JoinedUsers = new List<string>();
        private void TwitchClient_OnChatCommandReceived(object sender, TwitchLib.Events.Client.OnChatCommandReceivedArgs e) {
            if (e.Command.Command == Command) {
                if (JoinedUsers.Exists(user => user == e.Command.ChatMessage.Username)) {
                    JoinedUsers.Add(e.Command.ChatMessage.Username);

                    new Switch<string>(e.Command.ArgumentsAsString)
                        .Case(s => s == Option1, s => {
                            CountOption1.Value++;
                        })
                       .Case(s => s == Option2, s => {
                           CountOption2.Value++;
                       })
                       .Case(s => s == Option3, s => {
                           CountOption3.Value++;
                       })
                       .Case(s => s == Option4, s => {
                           CountOption4.Value++;
                       })
                       .Case(s => s == Option5, s => {
                           CountOption5.Value++;
                       })
                       .Case(s => s == Option6, s => {
                           CountOption6.Value++;
                       })
                       .Case(s => s == Option7, s => {
                           CountOption7.Value++;
                       })
                       .Case(s => s == Option8, s => {
                           CountOption8.Value++;
                       });
                }
            }
        }

        #region DrawCharts
        private void DrawTChart() {
            if (tChartCollection == null)
                tChartCollection = new SeriesCollection();

            if (!String.IsNullOrEmpty(Option1)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Option1,
                        Values = new ChartValues<ObservableValue> { CountOption1 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Option2)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Option2,
                        Values = new ChartValues<ObservableValue> { CountOption2 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Option3)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Option3,
                        Values = new ChartValues<ObservableValue> { CountOption3 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Option4)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Option4,
                        Values = new ChartValues<ObservableValue> { CountOption4 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Option5)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Option5,
                        Values = new ChartValues<ObservableValue> { CountOption5 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Option6)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Option6,
                        Values = new ChartValues<ObservableValue> { CountOption6 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Option7)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Option7,
                        Values = new ChartValues<ObservableValue> { CountOption7 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Option8)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Option8,
                        Values = new ChartValues<ObservableValue> { CountOption8 },
                        DataLabels = true
                    });
            }
        }

        private void DrawDChart() {
            if (dChartCollection == null)
                dChartCollection = new SeriesCollection();

            if (!String.IsNullOrEmpty(Option1)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Option1,
                        Values = new ChartValues<ObservableValue> { CountOption1 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Option2)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Option2,
                        Values = new ChartValues<ObservableValue> { CountOption2 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Option3)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Option3,
                        Values = new ChartValues<ObservableValue> { CountOption3 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Option4)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Option4,
                        Values = new ChartValues<ObservableValue> { CountOption4 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Option5)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Option5,
                        Values = new ChartValues<ObservableValue> { CountOption5 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Option6)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Option6,
                        Values = new ChartValues<ObservableValue> { CountOption6 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Option7)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Option7,
                        Values = new ChartValues<ObservableValue> { CountOption7 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Option8)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Option8,
                        Values = new ChartValues<ObservableValue> { CountOption8 },
                        DataLabels = true
                    });
            }
        }

        private void DrawBChart() {
            if (bChartCollection == null)
                bChartCollection = new SeriesCollection();

            bChartCollection.Add(
                new ColumnSeries {
                    Values = new ChartValues<ObservableValue>
                    {
                        CountOption1,
                        CountOption2,
                        CountOption3,
                        CountOption4,
                        CountOption5,
                        CountOption6,
                        CountOption7,
                        CountOption8,
                    },
                    DataLabels = true,
                    //LabelPoint = point => point.ToString()
                });

            OptionNames = new[]
            {
                Option1,
                Option2 != null ? Option2 : null,
                Option3 != null ? Option3 : null,
                Option4 != null ? Option4 : null,
                Option5 != null ? Option5 : null,
                Option6 != null ? Option6 : null,
                Option7 != null ? Option7 : null,
                Option8 != null ? Option8 : null,
            };
            Formatter = value => value + " votes";
        }

        public Func<int, string> Formatter { get; set; }

        #endregion DrawCharts

        private void stopVoting(Object sender, EventArgs e) {
            IsStarted = false;
            Client.Client.ClientBBB.TwitchClientBBB.OnChatCommandReceived -= TwitchClient_OnChatCommandReceived;
            tChartCollection = null;
            dChartCollection = null;
            bChartCollection = null;

            CountOption1.Value = 0;
            CountOption2.Value = 0;
            CountOption3.Value = 0;
            CountOption4.Value = 0;
            CountOption5.Value = 0;
            CountOption6.Value = 0;
            CountOption7.Value = 0;
            CountOption8.Value = 0;
        }
    }

    public static class Extension {
        public class Switch<T> {
            public Switch(T o) {
                Object = o;
            }
            public T Object { get; private set; }
        }
    }

    public static class SwitchExtensions {
        public static AivaBot.ViewModels.Extension.Switch<T> Case<T>(this Switch<T> switchObject, T type, Action<T> action) {
            return Case<T>(switchObject, type, action, false);
        }
        public static Switch<T> Case<T>(this Switch<T> switchObject, Func<T, bool> function, Action<T> action) {
            return Case(switchObject, function, action, false);
        }
        public static Switch<T> Case<T>(this Switch<T> switchObject, T type, Action<T> action,
            bool fallThrough) {
            return Case<T>(switchObject, x => object.Equals(x, type), action, fallThrough);
        }
        public static Switch<T> Case<T>(this Switch<T> switchObject, Func<T, bool> function,
            Action<T> action, bool fallThrough) {
            if (switchObject == null) {
                return null;
            }
            else if (function(switchObject.Object)) {
                action(switchObject.Object);
                return fallThrough ? switchObject : null;
            }
            return switchObject;
        }
        public static void Default<T>(this Switch<T> switchObject, Action<T> action) {
            if (switchObject != null) {
                action(switchObject.Object);
            }
        }
    }
}
