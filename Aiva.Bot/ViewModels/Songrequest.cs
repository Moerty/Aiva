using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Bot.ViewModels {
    [PropertyChanged.ImplementPropertyChanged]
    public class Songrequest {
        public string Command { get; set; }

        private bool _IsEnabled;
        public bool IsEnabled {
            get {
                return _IsEnabled;
            }
            set {
                _IsEnabled = value;

                if(value) {
                    Handler.EnableSongrequest(Command);
                }
                else {
                    Handler.DisableSongrequest();
                }
            }
        }

        Extensions.Songrequest.SongrequestHandler Handler { get; set; }

        public Songrequest() {
            
        }
    }
}
