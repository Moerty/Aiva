using PropertyChanged;
using System;
using System.Drawing;
using TwitchLib.Enums;

namespace Aiva.Models.Chat {
    [AddINotifyPropertyChangedInterface]
    public class Message {
        public bool IsBroadcaster { get; set; }
        public bool IsMe { get; set; }
        public bool IsModerator { get; set; }
        public bool IsTurbo { get; set; }
        public int SubscribedMonthCount { get; set; }
        public bool IsSubscriber { get; set; }
        public UserType UserType { get; set; }
        public Color Color { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string MessageText { get; set; }
    }
}