using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aiva.Core.Client.Internal.Commands.ModCommands {
    public class Handler {
        #region Models
        public Currency Currency;
        #endregion Models

        #region Contructor
        public Handler() {
            Currency = new Currency();
        }
        #endregion Constructor
    }
}