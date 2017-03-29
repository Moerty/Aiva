using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackBoxBot.ViewModels
{
    [PropertyChanged.ImplementPropertyChanged]
    class SettingsViewModel
    {
        private bool _LogChat = false;
        public bool LogChat
        {
            get
            {
                return _LogChat;
            }
            set
            {
                if(_LogChat)
                {
                    // Ausschalten
                    Client.Client.ClientBBB.TwitchClientBBB.OnMessageReceived -= Database.ChatHandler.MessageReceived;
                    _LogChat = value;
                }
                else
                {
                    // Einschalten
                    Client.Client.ClientBBB.TwitchClientBBB.OnMessageReceived += Database.ChatHandler.MessageReceived;
                    _LogChat = value;
                }
            }
        }

        public SettingsViewModel()
        {
            
        }
    }
}
