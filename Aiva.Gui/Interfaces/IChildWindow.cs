using System;
using System.Windows.Input;

namespace Aiva.Gui.Interfaces {
    public interface IChildWindow {
        ICommand SubmitCommand { get; set; }
        event EventHandler CloseEvent;
        bool IsCompleted();
    }
}