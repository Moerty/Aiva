using Aiva.Gui.Interfaces;
using System;
using System.Windows.Input;

namespace Aiva.Gui.ViewModels.ChildWindows.SpamProtection {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Caps : IChildWindow {
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

        public bool MaxUpperCharactersConsecurivelyActive {
            get {
                return _handler.MaxUpperCharactersConsecurivelyActive;
            }
            set {
                _handler.MaxUpperCharactersConsecurivelyActive = value;
            }
        }

        public int MaxUpperCharactersConsecurivelyInMessage {
            get {
                return _handler.MaxUpperCharactersConsecurivelyInMessage;
            }
            set {
                _handler.MaxUpperCharactersConsecurivelyInMessage = value;
            }
        }

        public bool MaxCharactersInAWordActive {
            get {
                return _handler.MaxCharactersInAWordActive;
            }
            set {
                _handler.MaxCharactersInAWordActive = value;
            }
        }

        public int MaxCharactersInAWord {
            get {
                return _handler.MaxCharactersInAWord;
            }
            set {
                _handler.MaxCharactersInAWord = value;
            }
        }

        public bool MoreUpperThanLowerCharactersCheck {
            get {
                return _handler.MoreUpperThanLowerCharactersCheck;
            }
            set {
                _handler.MoreUpperThanLowerCharactersCheck = value;
            }
        }

        // has to be there cause interface, but we dont need a return value, cause this is not required in this context
        public ICommand SubmitCommand { get; set; }

        public event EventHandler CloseEvent;

        private readonly Extensions.SpamProtection.Caps _handler;

        public Caps() {
            _handler = new Extensions.SpamProtection.Caps();
        }

        public bool IsCompleted() {
            return true;
        }

        protected virtual void OnCloseEvent(EventArgs e) {
            CloseEvent?.Invoke(this, e);
        }
    }
}