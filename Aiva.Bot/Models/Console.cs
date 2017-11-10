using System.Collections.ObjectModel;
using System.Linq;

namespace Aiva.Bot.Models {
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class Console {
        public ObservableCollection<MessageModel> Messages { get; set; }

        private string _MessageToSend;

        public string MessageToSend {
            get {
                return _MessageToSend;
            }
            set {
                _MessageToSend = value;

                if (_MessageToSend.Any()) {
                    CanSendMessage = true;
                } else {
                    CanSendMessage = false;
                }
            }
        }

        public bool CanSendMessage { get; set; }
    }

    public class MessageModel {
        public bool IsUserMod { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public long TwitchID { get; set; }
    }
}