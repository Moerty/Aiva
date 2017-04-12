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
        public Models.VotingModel Model { get; set; }

        public ICommand StartVotingCommand { get; set; } = new RoutedCommand();
        public ICommand StopVotingCommand { get; set; } = new RoutedCommand();

        public SeriesCollection tChartCollection { get; set; }
        public SeriesCollection dChartCollection { get; set; }
        public SeriesCollection bChartCollection { get; set; }
        public string[] OptionNames { get; set; }

        public VotingViewModel() {
            // CreateModels
            CreateModels();

            CommandManager.RegisterClassCommandBinding(new MahApps.Metro.Controls.MetroContentControl().GetType(), new CommandBinding(StartVotingCommand, StartVoting));
            CommandManager.RegisterClassCommandBinding(new MahApps.Metro.Controls.MetroContentControl().GetType(), new CommandBinding(StopVotingCommand, StopVoting));

            tChartCollection = new SeriesCollection();
            dChartCollection = new SeriesCollection();
            bChartCollection = new SeriesCollection();
        }

        private void CreateModels() {
            Model = new Models.VotingModel {
                IsStarted = false,
                ChartPie = true,
                ChartBasic = false,
                ChartDonut = false,
            };
        }

        public Func<ChartPoint, string> PointLabel { get; set; }


        private void StartVoting(Object sender, EventArgs e) {
            if (String.IsNullOrEmpty(Model.Command)) return;
            tChartCollection = new SeriesCollection();
            DrawTChart();
            DrawDChart();
            DrawBChart();
            Model.IsStarted = true;
            Client.Client.ClientBBB.TwitchClientBBB.OnChatCommandReceived += TwitchClient_OnChatCommandReceived;
        }


        List<string> JoinedUsers = new List<string>();
        private void TwitchClient_OnChatCommandReceived(object sender, TwitchLib.Events.Client.OnChatCommandReceivedArgs e) {
            if (String.Compare(e.Command.Command, Model.Command, true) == 0) {
                if (!JoinedUsers.Exists(user => user == e.Command.ChatMessage.Username)) {
                    JoinedUsers.Add(e.Command.ChatMessage.Username);

                    new Switch<string>(e.Command.ArgumentsAsString)
                        .Case(s => String.Compare(s, Model.Option1, true) == 0, s => {
                            Model.CountOption1.Value++;
                        })
                       .Case(s => String.Compare(s, Model.Option2, true) == 0, s => {
                           Model.CountOption2.Value++;
                       })
                       .Case(s => String.Compare(s, Model.Option3, true) == 0, s => {
                           Model.CountOption3.Value++;
                       })
                       .Case(s => String.Compare(s, Model.Option4, true) == 0, s => {
                           Model.CountOption4.Value++;
                       })
                       .Case(s => String.Compare(s, Model.Option5, true) == 0, s => {
                           Model.CountOption5.Value++;
                       })
                       .Case(s => String.Compare(s, Model.Option6, true) == 0, s => {
                           Model.CountOption6.Value++;
                       })
                       .Case(s => String.Compare(s, Model.Option7, true) == 0, s => {
                           Model.CountOption7.Value++;
                       })
                       .Case(s => String.Compare(s, Model.Option8, true) == 0, s => {
                           Model.CountOption8.Value++;
                       });
                }
            }
        }

        #region DrawCharts
        private void DrawTChart() {
            if (tChartCollection == null)
                tChartCollection = new SeriesCollection();

            if (!String.IsNullOrEmpty(Model.Option1)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option1,
                        Values = new ChartValues<ObservableValue> { Model.CountOption1 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Model.Option2)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option2,
                        Values = new ChartValues<ObservableValue> { Model.CountOption2 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Model.Option3)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option3,
                        Values = new ChartValues<ObservableValue> { Model.CountOption3 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Model.Option4)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option4,
                        Values = new ChartValues<ObservableValue> { Model.CountOption4 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Model.Option5)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option5,
                        Values = new ChartValues<ObservableValue> { Model.CountOption5 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Model.Option6)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option6,
                        Values = new ChartValues<ObservableValue> { Model.CountOption6 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Model.Option7)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option7,
                        Values = new ChartValues<ObservableValue> { Model.CountOption7 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Model.Option8)) {
                tChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option8,
                        Values = new ChartValues<ObservableValue> { Model.CountOption8 },
                        DataLabels = true
                    });
            }
        }

        private void DrawDChart() {
            if (dChartCollection == null)
                dChartCollection = new SeriesCollection();

            if (!String.IsNullOrEmpty(Model.Option1)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option1,
                        Values = new ChartValues<ObservableValue> { Model.CountOption1 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Model.Option2)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option2,
                        Values = new ChartValues<ObservableValue> { Model.CountOption2 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Model.Option3)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option3,
                        Values = new ChartValues<ObservableValue> { Model.CountOption3 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Model.Option4)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option4,
                        Values = new ChartValues<ObservableValue> { Model.CountOption4 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Model.Option5)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option5,
                        Values = new ChartValues<ObservableValue> { Model.CountOption5 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Model.Option6)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option6,
                        Values = new ChartValues<ObservableValue> { Model.CountOption6 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Model.Option7)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option7,
                        Values = new ChartValues<ObservableValue> { Model.CountOption7 },
                        DataLabels = true
                    });
            }

            if (!String.IsNullOrEmpty(Model.Option8)) {
                dChartCollection.Add(
                    new PieSeries {
                        Title = Model.Option8,
                        Values = new ChartValues<ObservableValue> { Model.CountOption8 },
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
                        Model.CountOption1,
                        Model.CountOption2,
                        Model.CountOption3,
                        Model.CountOption4,
                        Model.CountOption5,
                        Model.CountOption6,
                        Model.CountOption7,
                        Model.CountOption8,
                    },
                    DataLabels = true,
                    //LabelPoint = point => point.ToString()
                });

            OptionNames = new[]
            {
                Model.Option1,
                Model.Option2 != null ? Model.Option2 : null,
                Model.Option3 != null ? Model.Option3 : null,
                Model.Option4 != null ? Model.Option4 : null,
                Model.Option5 != null ? Model.Option5 : null,
                Model.Option6 != null ? Model.Option6 : null,
                Model.Option7 != null ? Model.Option7 : null,
                Model.Option8 != null ? Model.Option8 : null,
            };
            Formatter = value => value + " votes";
        }

        public Func<int, string> Formatter { get; set; }

        #endregion DrawCharts

        private void StopVoting(Object sender, EventArgs e) {
            Model.IsStarted = false;
            Client.Client.ClientBBB.TwitchClientBBB.OnChatCommandReceived -= TwitchClient_OnChatCommandReceived;
            tChartCollection = null;
            dChartCollection = null;
            bChartCollection = null;

            Model.CountOption1.Value = 0;
            Model.CountOption2.Value = 0;
            Model.CountOption3.Value = 0;
            Model.CountOption4.Value = 0;
            Model.CountOption5.Value = 0;
            Model.CountOption6.Value = 0;
            Model.CountOption7.Value = 0;
            Model.CountOption8.Value = 0;
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
