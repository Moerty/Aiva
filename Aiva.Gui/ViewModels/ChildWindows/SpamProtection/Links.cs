using Aiva.Gui.Interfaces;
using System;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.ChildWindows.SpamProtection {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Links : IChildWindow {
        public string Whitelist { get; set; }

        public bool Active {
            get {
                return _handler.Active;
            }
            set {
                _handler.Active = value;
            }
        }

        public int TimeoutTimeInSeconds {
            get {
                return _handler.TimeoutTimeInSeconds;
            }
            set {
                _handler.TimeoutTimeInSeconds = value;
            }
        }

        public ICommand SubmitCommand { get; set; }

        public event EventHandler CloseEvent;

        private bool _isCompleted;
        private readonly Extensions.SpamProtection.Links _handler;

        public bool IsCompleted() {
            return _isCompleted;
        }

        public Links() {
            _handler = new Extensions.SpamProtection.Links();
            SubmitCommand = new Internal.RelayCommand(cancel => Submit());
        }

        private void Submit() {
            _isCompleted = true;
            CloseEvent.Invoke(this, EventArgs.Empty);
        }
    }
}