using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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
    class BetsViewModel
    {

        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        public ICommand Start { get; set; } = new RoutedCommand();
        public ICommand Stop { get; set; } = new RoutedCommand();
        public ICommand PayOutCommand { get; set; } = new RoutedCommand();

        public BlackBoxBot.Models.BetsModel Model { get; set; }
        public Bets.BetsHandler BetsHandler { get; set; }

        public BetsViewModel()
        {
            // Initialize
            CreateModels();
            BetsHandler = new BlackBoxBot.Bets.BetsHandler();

            // Command
            CommandManager.RegisterClassCommandBinding(new MahApps.Metro.Controls.MetroContentControl().GetType(), new CommandBinding(Start, StartBet));

            Labels = new[] { "Wetter", "Einsatz", };
            Formatter = val => val.ToString("P").Replace(".0", "");
        }

        private bool CanPayOut(object obj)
        {
            return true;
        }

        private void PayOneTeam1(object sender)
        {
            if(!BetsHandler.IsStarted)
                BetsHandler.Current.PayOut('a', WinningMultiplicator);
        }

        private void PayOneTeam2(object sender)
        {
            if(!BetsHandler.IsStarted)
                BetsHandler.Current.PayOut('b', WinningMultiplicator);
        }

        private void StartBet(object sender = null, ExecutedRoutedEventArgs e = null)
        {
            
            if (!BetsHandler.IsStarted)
            {
                if(String.IsNullOrEmpty(CommandName)) { BetsHandler.IsStarted = !BetsHandler.IsStarted; return; }
                BetsHandler.IsStarted = true;
                BetsHandler.Current = new Bets.CurrentBet(MinutesToEnd, CommandName);
            }
            else
            {
                BetsHandler.IsStarted = false;
                BetsHandler.Current.EndRegistration();
            }
        }

        private void CreateModels()
        {
            Model = new BlackBoxBot.Models.BetsModel();
            
            Model.Text = new BlackBoxBot.Models.BetsModel.TextModel
            {
                CommandWatermark = Config.Language.Instance.GetString("BetsCommandWatermark"),
                TextBoxTextTimeForBet = Config.Language.Instance.GetString("BetsTextBoxTextTimeForBet"),
                StartStopButtonText = Config.Language.Instance.GetString("BetsStartStopButtonText"),
                PayOutButtonText = Config.Language.Instance.GetString("BetsPayOutButtonText"),
                ExpanderUsersName = Config.Language.Instance.GetString("BetsExpanderUsersName"),
                BetOption1Text = Config.Language.Instance.GetString("BetsBetOption1Text"),
                BetOption2Text = Config.Language.Instance.GetString("BetsBetOption2Text"),
                CountName = Config.Language.Instance.GetString("BetsCountName"),
                ValueName = Config.Language.Instance.GetString("BetsValueName"),
                GridBetterHeaderName = Config.Language.Instance.GetString("BetsGridBetterHeaderName"),
                GridBetValueHeaderName = Config.Language.Instance.GetString("BetsGridBetValueHeaderName"),
                GridTeamHeaderName = Config.Language.Instance.GetString("BetsGridTeamHeaderName"),
            };

            // PayOut Drop down Menu
            Model.DropDownMenu.Add(
                new Models.BetsModel.DropDownButtonModel
                {
                    Name = "Team 1",
                    Command = new RelayCommand(CanPayOut, PayOneTeam1)
                });
            Model.DropDownMenu.Add(
                new Models.BetsModel.DropDownButtonModel
                {
                    Name = "Team 2",
                    Command = new RelayCommand(CanPayOut, PayOneTeam2),
                });
        }

        private int _MinutesToEnd = -1;
        public int MinutesToEnd
        {
            get
            {
                if (_MinutesToEnd == -1)
                {
                    _MinutesToEnd = 2;
                    return _MinutesToEnd;
                }

                return _MinutesToEnd;
            }
            set
            {
                if (value < 0) return;

                _MinutesToEnd = value;
            }
        }

        public string CommandName { get; set; }
        public int WinningMultiplicator { get; set; } = 1;
        


    }

    public class RelayCommand : ICommand
    {
        private Predicate<object> _canExecute;
        private Action<object> _execute;

        public RelayCommand(Predicate<object> canExecute, Action<object> execute)
        {
            this._canExecute = canExecute;
            this._execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
