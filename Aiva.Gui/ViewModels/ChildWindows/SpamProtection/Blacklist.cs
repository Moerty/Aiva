using Aiva.Gui.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.ChildWindows.SpamProtection {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Blacklist : IChildWindow {
        public string WordToAdd { get; set; }
        public Core.Database.Storage.BlacklistedWords SelectedWord { get; set; }
        public ObservableCollection<Core.Database.Storage.BlacklistedWords> Words { get; set; }

        public int TimeoutTime {
            get {
                return _handler.TimeoutTime;
            }
            set {
                _handler.TimeoutTime = value;
            }
        }

        public ICommand SubmitCommand { get; set; }
        public ICommand AddWordCommand { get; set; }
        public ICommand RemoveCommand { get; set; }

        public event EventHandler CloseEvent;

        private readonly Extensions.SpamProtection.Blacklist _handler;

        public Blacklist() {
            _handler = new Extensions.SpamProtection.Blacklist();
            Words = new ObservableCollection<Core.Database.Storage.BlacklistedWords>(_handler.GetWords());
            AddWordCommand = new Internal.RelayCommand(add => AddWord(), add => !string.IsNullOrEmpty(WordToAdd)
                                                                                && IsNotInList());
            RemoveCommand = new Internal.RelayCommand(remove => Remove(), remove => SelectedWord != null);
        }

        private void Remove() {
            _handler.RemoveWord(SelectedWord.Word);
            SelectedWord = null;
        }

        private bool IsNotInList() {
            return _handler.IsWordInList(WordToAdd, true);
        }

        private void AddWord() {
            _handler.AddWordToBlacklist(WordToAdd);
            WordToAdd = string.Empty;
            Words = new ObservableCollection<Core.Database.Storage.BlacklistedWords>(_handler.GetWords());
        }

        public bool IsCompleted() {
            return true;
        }

        protected virtual void OnCloseEvent(EventArgs e) {
            CloseEvent?.Invoke(this, e);
        }
    }
}