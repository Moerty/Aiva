using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Extensions.Models {
    public class Songrequest {
        public enum YoutubePlayerState
        {
            unstarted,
            ended,
            playing,
            paused,
            buffering,
            videoCued,
            unknownvalue
        }


        public interface Player
        {


            void AddSong();
            void DeleteSong();
            void ChangeSong();
        }
    }

}
