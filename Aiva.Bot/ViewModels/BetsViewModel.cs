using Aiva.Core.Config;
using AivaBot.Bets;
using System;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    class BetsViewModel {
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public ICommand Start { get; set; } = new RoutedCommand();
        public ICommand Stop { get; set; } = new RoutedCommand();

        public Models.BetsModel Model { get; set; }
        public BetsHandler BetsHandler { get; set; }
        public string CommandName { get; set; }
        public int WinningMultiplicator { get; set; } = 1;

        public BetsViewModel() {
            // Initialize
            CreateModels();
            BetsHandler = new BetsHandler();

            // Command
            CommandManager.RegisterClassCommandBinding(new MahApps.Metro.Controls.MetroContentControl().GetType(), new CommandBinding(Start, StartBet));

            Labels = new[] { "Wetter", "Einsatz", };
            Formatter = val => val.ToString("P").Replace(".0", "");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartBet(object sender = null, ExecutedRoutedEventArgs e = null) {

            if (e != null) {
                if (e.Source is System.Windows.Controls.Primitives.ToggleButton source) {
                    if (source.IsChecked.HasValue) {
                        if (source.IsChecked.Value) {
                            if (String.IsNullOrEmpty(CommandName)) { BetsHandler.IsStarted = !BetsHandler.IsStarted; return; }
                            BetsHandler.Current = new CurrentBet(new TimeSpan(0, MinutesToEnd, 0), CommandName);
                        } else {
                            BetsHandler.IsStarted = false;
                            BetsHandler.Current.EndRegistration();
                        }
                    }
                }
            }
        }

        private void CreateModels() {
            Model = new Aiva.Bot.Models.BetsModel {
                Text = new Aiva.Bot.Models.BetsModel.TextModel {
                    CommandWatermark = LanguageConfig.Instance.GetString("BetsCommandWatermark"),
                    TextBoxTextTimeForBet = LanguageConfig.Instance.GetString("BetsTextBoxTextTimeForBet"),
                    StartStopButtonText = LanguageConfig.Instance.GetString("BetsStartStopButtonText"),
                    PayOutButtonText = LanguageConfig.Instance.GetString("BetsPayOutButtonText"),
                    ExpanderUsersName = LanguageConfig.Instance.GetString("BetsExpanderUsersName"),
                    BetOption1Text = LanguageConfig.Instance.GetString("BetsBetOption1Text"),
                    BetOption2Text = LanguageConfig.Instance.GetString("BetsBetOption2Text"),
                    CountName = LanguageConfig.Instance.GetString("BetsCountName"),
                    ValueName = LanguageConfig.Instance.GetString("BetsValueName"),
                    GridBetterHeaderName = LanguageConfig.Instance.GetString("BetsGridBetterHeaderName"),
                    GridBetValueHeaderName = LanguageConfig.Instance.GetString("BetsGridBetValueHeaderName"),
                    GridTeamHeaderName = LanguageConfig.Instance.GetString("BetsGridTeamHeaderName"),
                }
            };

            // PayOut Drop down Menu
            Model.DropDownMenu.Add(
                new Models.BetsModel.DropDownButtonModel {
                    Name = "Team 1",
                    Command = new RelayCommand(CanPayOut, PayOneTeam1)
                });
            Model.DropDownMenu.Add(
                new Models.BetsModel.DropDownButtonModel {
                    Name = "Team 2",
                    Command = new RelayCommand(CanPayOut, PayOneTeam2),
                });
        }

        private int _MinutesToEnd = -1;
        public int MinutesToEnd {
            get {
                if (_MinutesToEnd == -1) {
                    _MinutesToEnd = 2;
                    return _MinutesToEnd;
                }

                return _MinutesToEnd;
            }
            set {
                if (value < 0) return;

                _MinutesToEnd = value;
            }
        }

        #region PayOut
        private bool CanPayOut(object obj) {
            return !BetsHandler.IsStarted;
        }

        /// <summary>
        /// Payout Team1
        /// </summary>
        /// <param name="sender"></param>
        private void PayOneTeam1(object sender) {
            if (!BetsHandler.IsStarted)
                BetsHandler.Current.PayOut('a', WinningMultiplicator);
        }

        /// <summary>
        /// Payout Team2
        /// </summary>
        /// <param name="sender"></param>
        private void PayOneTeam2(object sender) {
            if (!BetsHandler.IsStarted)
                BetsHandler.Current.PayOut('b', WinningMultiplicator);
        }
        #endregion
    }

    /// <summary>
    /// Class for DropDownButton
    /// </summary>
    public class RelayCommand : ICommand {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        public RelayCommand(Predicate<object> canExecute, Action<object> execute) {
            this._canExecute = canExecute;
            this._execute = execute;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return _canExecute(parameter);
        }

        public void Execute(object parameter) {
            _execute(parameter);
        }
    }
}
