using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bets.Exceptions
{
    [Serializable]
    class ExceptionValueNotValid : Exception
    {
        public ExceptionValueNotValid()
        { }

        public ExceptionValueNotValid(string message)
            : base(message)
        { }

        public ExceptionValueNotValid(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
