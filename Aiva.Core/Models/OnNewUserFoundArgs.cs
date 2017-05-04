using System;
using System.Collections.Generic;
using TwitchLib.Models.API.User;

namespace Aiva.Core.Models {
    public class OnNewUserFoundArgs : EventArgs {
        public List<User> User { get; set; }
    }
}