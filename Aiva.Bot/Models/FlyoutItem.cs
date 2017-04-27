using MahApps.Metro.Controls;

namespace Aiva.Bot.Models {
    public class FlyoutItem : Flyout {
        public new string Header { get; set; }
        public new bool IsOpen { get; set; }
        public new Position Position { get; set; }
    }
}