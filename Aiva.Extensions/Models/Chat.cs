using System;
using System.Drawing;
using TwitchLib.Enums;

namespace Aiva.Extensions.Models {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Chat {
        [PropertyChanged.AddINotifyPropertyChangedInterface]
        public class MessageModel {
            //
            // Zusammenfassung:
            //     Chat message from broadcaster identifier flag
            public bool IsBroadcaster { get; set; }

            //
            // Zusammenfassung:
            //     Chat message /me identifier flag.
            public bool IsMe { get; set; }

            //
            // Zusammenfassung:
            //     Channel specific moderator status.
            public bool IsModerator { get; set; }

            //
            // Zusammenfassung:
            //     Twitch site-wide turbo status.
            public bool IsTurbo { get; set; }

            //
            // Zusammenfassung:
            //     Number of months a person has been subbed.
            public int SubscribedMonthCount { get; set; }

            //
            // Zusammenfassung:
            //     Channel specific subscriber status.
            public bool IsSubscriber { get; set; }

            //
            // Zusammenfassung:
            //     User type can be viewer, moderator, global mod, admin, or staff
            public UserType UserType { get; set; }

            //
            // Zusammenfassung:
            //     Twitch chat message contents.
            public string Message { get; set; }

            //
            // Zusammenfassung:
            //     Property representing HEX color as a System.Drawing.Color object.
            public System.Drawing.Color Color { get; set; }

            //
            // Zusammenfassung:
            //     Case-sensitive username of sender of chat message.
            public string DisplayName { get; set; }

            //
            // Zusammenfassung:
            //     Username of sender of chat message.
            public string Username { get; set; }

            //
            // Zusammenfassung:
            //     Twitch-unique integer assigned on per account basis.
            public string UserId { get; set; }

            ///
            /// Zusammenfassung:
            ///     Timestamp of the message.
            public DateTime Timestamp { get; set; }
        }

        [PropertyChanged.AddINotifyPropertyChangedInterface]
        public class Viewers {
            public bool IsMod { get; set; }
            public bool IsSub { get; set; }
            public string Name { get; set; }
            public string TwitchID { get; set; }
            public string Type { get; set; }
            public Color ChatNameColor { get; set; }
        }

        public enum SortDirectionListView {
            Admin,
            Mod,
            Subscriber,
            Follower,
            Viewer
        }
    }
}