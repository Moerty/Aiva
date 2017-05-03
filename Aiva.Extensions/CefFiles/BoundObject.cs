using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Aiva.Extensions.Models.Songrequest;

namespace Aiva.Extensions.CefFiles
{
    public class BoundObject
    {
        public event EventHandler PlayerLoadingDone;
        public event EventHandler PlayerQualityChanged;
        public event EventHandler<Models.Songrequest.YoutubePlayerState> PlayerPlayingChanged;

        public void PlayerLoaded()
        {
            if (PlayerLoadingDone != null) {
                PlayerLoadingDone(this, new EventArgs());
            }
        }

        public void qualityChanged()
        {
            PlayerQualityChanged?.Invoke(this, new EventArgs());
        }

        public void PlayingChanged(int state)
        {
            PlayerPlayingChanged?.Invoke(this, state.ParseToYoutubeState());
        }
    }

    public static class YoutubeStateExtensions
    {
        public static YoutubePlayerState ParseToYoutubeState(this int state)
        {
            switch ((int)state) {
                case -1:
                    return YoutubePlayerState.unstarted;
                case 0:
                    return YoutubePlayerState.ended;
                case 1:
                    return YoutubePlayerState.playing;
                case 2:
                    return YoutubePlayerState.paused;
                case 3:
                    return YoutubePlayerState.buffering;
                case 5:
                    return YoutubePlayerState.videoCued;
                default:
                    return YoutubePlayerState.unknownvalue;
            }
        }
    }
}
