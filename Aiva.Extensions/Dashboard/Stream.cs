using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Extensions.Dashboard {
    public class Stream {

        /// <summary>
        /// Change the Stream Title
        /// </summary>
        /// <param name="StreamTitle">Title</param>
        public static async void ChangeTitle(string StreamTitle) {
            await TwitchLib.TwitchApi.Streams.UpdateStreamTitleAsync(StreamTitle, Core.Client.AivaClient.Client.Channel);
        }

        /// <summary>
        /// Get active Viewers
        /// </summary>
        /// <returns>Returns active Viewers</returns>
        public static async Task<int> GetActiveViewers() {
            var isStreaming = TwitchLib.TwitchApi.Streams.BroadcasterOnline(Core.Client.AivaClient.Client.Channel);
            if (isStreaming) {
                var viewers = await TwitchLib.TwitchApi.Streams.GetStreamAsync(Core.Client.AivaClient.Client.Channel);

                return viewers.Viewers;
            } else {
                return 0;
            }
        }

        /// <summary>
        /// Get last Follower
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetLastFollower() {
            var follower = await TwitchLib.TwitchApi.Follows.GetFollowersAsync(
                Core.Client.AivaClient.Client.Channel,
                1,
                null,
                TwitchLib.Enums.SortDirection.Descending);

            return follower.Followers[0].User.Name;
        }

        /// <summary>
        /// Get Channelname and Game
        /// </summary>
        /// <returns>Channel ; Game</returns>
        public static async Task<Tuple<string, string, int>> GetChannelDetails() {
            var channel = await TwitchLib.TwitchApi.Channels.GetChannelAsync(Core.Client.AivaClient.Client.Channel);

            return new Tuple<string, string, int>(channel.Status, channel.Game, channel.Views);
        }

        public static void ShowCommercial(int lenth) {
            var isOnline = TwitchLib.TwitchApi.Streams.BroadcasterOnline(Core.Client.AivaClient.Client.Channel);

            if (isOnline) {
                switch (lenth) {
                    case 30:
                        TwitchLib.TwitchApi.Streams.RunCommercialAsync(TwitchLib.Enums.CommercialLength.Seconds30,
                            Core.Client.AivaClient.Client.Channel);
                        break;
                    case 60:
                        TwitchLib.TwitchApi.Streams.RunCommercialAsync(TwitchLib.Enums.CommercialLength.Seconds60,
                            Core.Client.AivaClient.Client.Channel);
                        break;
                    case 90:
                        TwitchLib.TwitchApi.Streams.RunCommercialAsync(TwitchLib.Enums.CommercialLength.Seconds90,
                            Core.Client.AivaClient.Client.Channel);
                        break;
                    case 120:
                        TwitchLib.TwitchApi.Streams.RunCommercialAsync(TwitchLib.Enums.CommercialLength.Seconds120,
                            Core.Client.AivaClient.Client.Channel);
                        break;
                    case 150:
                        TwitchLib.TwitchApi.Streams.RunCommercialAsync(TwitchLib.Enums.CommercialLength.Seconds150,
                            Core.Client.AivaClient.Client.Channel);
                        break;
                    case 180:
                        TwitchLib.TwitchApi.Streams.RunCommercialAsync(TwitchLib.Enums.CommercialLength.Seconds180,
                            Core.Client.AivaClient.Client.Channel);
                        break;
                }
            }
        }
    }
}
