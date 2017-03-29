using System;

namespace Bets.Exceptions
{
    [Serializable]
    class ExceptionParametersNotValid : Exception
    {
        public ExceptionParametersNotValid()
        { }

        public ExceptionParametersNotValid(string message)
            : base(message)
        { }

        public ExceptionParametersNotValid(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
