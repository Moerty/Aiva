using Aiva.Core.Config;
using Newtonsoft.Json;
using TwitchLib.Enums;

namespace Aiva.Core {
    public static class Helpers {
        /// <summary>
        /// Checks if a user is higher than mod
        /// </summary>
        /// <param name="userType"></param>
        /// <returns></returns>
        public static bool IsUserPermitted(this UserType userType) {
            return userType != UserType.Viewer && userType != UserType.Staff;
        }

        /// <summary>
        /// Helper class to serialize class to json
        /// </summary>
        /// <param name="self"></param>
        public static string ToJson(this Storage.Root self)
            => JsonConvert.SerializeObject(self, Storage.Converter.Settings);
    }
}