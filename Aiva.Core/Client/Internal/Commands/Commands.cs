using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Core.Client.Internal.Commands {
    public class Commands {
        #region Models
        public ModCommands.Handler ModCommands;
        #endregion Models

        #region Constructor
        public Commands() {
            ModCommands = new Internal.Commands.ModCommands.Handler();
        }
        #endregion Construtor
    }
}
