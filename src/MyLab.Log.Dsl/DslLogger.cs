using System;
using Microsoft.Extensions.Logging;

namespace MyLab.Log.Dsl
{
    class DslLogger : IDslLogger
    {
        private readonly ILogger _coreLogger;

        public DslLogger(ILogger coreLogger)
        {
            _coreLogger = coreLogger ?? throw new ArgumentNullException(nameof(coreLogger));
        }

        public DslExpression Debug(string message)
        {
            CheckMessageForWhitespace(message);

            return new DslExpression(_coreLogger, message, PredefinedLogLevels.Debug);
        }

        public DslExpression Action(string message)
        {
            CheckMessageForWhitespace(message);

            return new DslExpression(_coreLogger, message);
        }

        public DslExpression Warning(string message)
        {
            CheckMessageForWhitespace(message);

            return new DslExpression(_coreLogger, message, PredefinedLogLevels.Warning);
        }

        public DslExpression Warning(string message, Exception reasonException)
        {
            CheckMessageForWhitespace(message);

            return new DslExpression(_coreLogger, message, PredefinedLogLevels.Warning)
                .BecauseOf(reasonException);
        }

        public DslExpression Warning(Exception reasonException)
        {
            if (reasonException == null) throw new ArgumentNullException(nameof(reasonException));

            return new DslExpression(_coreLogger, reasonException.Message, PredefinedLogLevels.Warning)
                .BecauseOf(reasonException);
        }

        public DslExpression Error(string message)
        {
            CheckMessageForWhitespace(message);

            return new DslExpression(_coreLogger, message, PredefinedLogLevels.Error);
        }

        public DslExpression Error(string message, Exception reasonException)
        {
            CheckMessageForWhitespace(message);

            return new DslExpression(_coreLogger, message, PredefinedLogLevels.Error)
                .BecauseOf(reasonException);
        }

        public DslExpression Error(Exception reasonException)
        {
            if (reasonException == null) throw new ArgumentNullException(nameof(reasonException));

            return new DslExpression(_coreLogger, reasonException.Message, PredefinedLogLevels.Error)
                .BecauseOf(reasonException);
        }

        void CheckMessageForWhitespace(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                throw new ArgumentException("Value cannot be null or whitespace.", "message");
        }
    }
}