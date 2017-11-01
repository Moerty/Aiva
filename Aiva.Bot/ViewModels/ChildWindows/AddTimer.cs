using System;
using System.Windows.Input;

namespace Aiva.Bot.ViewModels.ChildWindows {

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class AddTimer {

        public string Name { get; set; }
        public string Text { get; set; }
        public int Interval { get; set; } = 15;
        public int Lines { get; set; } = 1;

        public ICommand SubmitCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public event EventHandler CloseEvent;

        public bool IsCompleted;

        public AddTimer() {
            SetCommands();
        }

        private void SetCommands() {
            SubmitCommand = new Internal.RelayCommand(submit => Submit());
            CancelCommand = new Internal.RelayCommand(cancel => Cancel());
        }

        private void Cancel() {
            CloseEvent(this, EventArgs.Empty);
        }

        private void Submit() {
            IsCompleted = true;
            CloseEvent(this, EventArgs.Empty);
        }
    }
}