using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackBoxBot.Models {
    [PropertyChanged.ImplementPropertyChanged]
    public class FirstStartModel {
        public MahApps.Metro.Controls.MetroContentControl Content { get; set; }
        public string OAuthToken { get; set; }
        public ObservableCollection<string> Scopes { get; set; }
        public bool GoogleAuth { get; set; } = false;
        public bool TwitchAuth { get; set; } = false;
        public string Channel { get; set; }
        public string ClientID { get; set; }
    }
}
