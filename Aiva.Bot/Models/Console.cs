using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Bot.Models {
    public class Console : INotifyPropertyChanged {
        public ObservableCollection<MessageModel> Messages { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

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

