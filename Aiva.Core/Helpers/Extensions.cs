using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.Enums;
using TwitchLib.Models.API.v5.Users;

namespace Aiva.Core.Helpers {
    public static class Extensions {

        /// <summary>
        /// Checks if a user is higher than mod
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static bool IsUserPermitted(this UserType userType) {
            return userType != UserType.Viewer && userType != UserType.Staff;
        }
    }
}
