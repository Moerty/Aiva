using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Windows.Data;

namespace Aiva.Bot.Internal {
    
    internal interface IFlyoutViewModel {
        string Header { get; set; }

        bool IsOpen { get; set; }

        Position Position { get; set; }

        bool IsModal { get; set; }

        Flyout Content { get; set; }
    }
}
