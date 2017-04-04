using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib;
using TwitchLib.Models.Client;
using TwitchLib.Events.Client;

namespace Client {
    public class Client {
        public static Client ClientBBB { get; set; }

        public TwitchClient TwitchClientBBB { get; set; }
        public bool SpamCheckActivated;

        public string Channel { get; set; }
        public string AccessToken { get; set; }

        public Client() {
            var username = Config.General.Config["General"]["BotName"];
            var oauthKey = Config.General.Config["Credentials"]["TwitchOAuth"];
            AccessToken = oauthKey;
            var channel = Config.General.Config["General"]["Channel"].ToLower();
            Channel = channel;

            // Set TwitchAPI Config
            TwitchApi.SetClientId(Config.General.Config["Credentials"]["TwitchClientID"]);
            TwitchApi.SetAccessToken(Config.General.Config["Credentials"]["TwitchOAuth"]);

            // Credentials
            var credentials = new ConnectionCredentials(username, oauthKey);

            var commandIdentifier = Convert.ToChar(Config.General.Config["General"]["CommandIdentifier"]);
            TwitchClientBBB = new TwitchClient(credentials, channel, commandIdentifier, '@', true, true);

            // Get initial events
            TwitchClientBBB.OnConnected += Client_OnConnected;

            // Set Tasks
            TwitchClientBBB = Tasks.Tasks.GetEvents(TwitchClientBBB);

            TwitchClientBBB.Connect();

            ClientBBB = this;
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e) {
            TwitchClientBBB.JoinChannel(Config.General.Config["General"]["Channel"].ToLower());
        }
    }
}