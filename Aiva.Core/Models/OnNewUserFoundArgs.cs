using System;
using System.Collections.Generic;
using TwitchLib.Enums;
using TwitchLib.Models.API.v5.Users;

namespace Aiva.Core.Models {
    public class OnNewUserFoundArgs : EventArgs {
        public List<User> Users { get; set; }
    }
}