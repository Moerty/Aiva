using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Extensions.Dashboard {
    public class Followers {
        public static List<Models.Dashboard.Followers> GetFollowers() {
            var follows = new List<Models.Dashboard.Followers>();

            var ApiResponse = new List<TwitchLib.Models.API.Follow.FollowersResponse>();
            ApiResponse.Add(TwitchLib.TwitchApi.Follows.GetFollowers(Aiva.Core.Client.AivaClient.Client.Channel, 100));

            if (ApiResponse[0] != null) {
                // Paging
                bool pagingActive = true;

                for (int i = 0; pagingActive; i++) {
                    //if (ApiResponse.Count > 1)
                        if (ApiResponse[i].TotalFollowerCount > i * 100) {
                            ApiResponse.Add(TwitchLib.TwitchApi.Follows.GetFollowers(Aiva.Core.Client.AivaClient.Client.Channel, 100, ApiResponse[i].Cursor));
                        } else {
                            pagingActive = false;
                            break;
                        }
                }

                // Add to return List
                foreach (var followerPage in ApiResponse) {
                    foreach (var follower in followerPage.Followers) {
                        follows.Add(
                        new Models.Dashboard.Followers {
                            CreatedAt = follower.CreatedAt,
                            Name = follower.User.Name,
                            Notifications = follower.Notifications
                        });
                    }
                }
            }

            return follows;
        }

        public async static Task<List<Models.Dashboard.Followers>> GetFollowersAsync() {
            return await Task.Run(() => GetFollowers());
        }
    }
}