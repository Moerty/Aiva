//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TwitchLib.Events.Client;

//namespace Aiva.Extensions.Songrequest {
//    public class SongrequestHandler {
//        /*
//         * 
//         * - Setting who can request songs
//            - max song duration
//            - max quene size
    
//            - cost sttings
    
//            - add playlist
//            - add video
//    */
//        public Player Player; 
//        public bool IsEnabled { get; set; }
//        public bool Autoplay { get; set; }
//        public string Command { get; set; }
//        public TwitchLib.Enums.UserType UserType { get; set; }

//        public void EnableSongrequest(string command) {
//            this.Command = command;
//            Player = new Player();
//            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived += OnSongrequestCommandReceived;
//        }

//        public void DisableSongrequest() {
//            Player = null;
//            Core.AivaClient.Instance.AivaTwitchClient.OnChatCommandReceived -= OnSongrequestCommandReceived;
//        }

//        private void OnSongrequestCommandReceived(object sender, OnChatCommandReceivedArgs e) {
//            if(String.Compare(e.Command.Command, Command, true) == 0) {
//                Player.SongList.Add(
//                    new Song(e.Command.ArgumentsAsString, e.Command.ChatMessage.Username) {
//                        TwitchID = Convert.ToInt64(e.Command.ChatMessage.UserId)
//                    });
//            }
//        }

//        public void StartSong(Song song) {
//            Player.ChangeSong(song, Autoplay);
//        }

//        public void StopSong() {
//            Player.StartStopMusic();
//        }
//    }
//}
