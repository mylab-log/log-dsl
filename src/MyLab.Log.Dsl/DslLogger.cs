using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace MyLab.Log.Dsl
{
    class DslLogger : IDslLogger
    {
        private readonly ILogger _coreLogger;

        public DslLogger(ILoggerFactory coreLoggerFactory)
        {
            if (coreLoggerFactory == null) throw new ArgumentNullException(nameof(coreLoggerFactory));

            _coreLogger = coreLoggerFactory.CreateLogger("");
        }

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

        public DslExpression Error(string message)
        {
            CheckMessageForWhitespace(message);

            return new DslExpression(_coreLogger, message, PredefinedLogLevels.Error);
        }

        void CheckMessageForWhitespace(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                throw new ArgumentException("Value cannot be null or whitespace.", "val");
        }
    }

    class DslLogger<TCategoryName> : DslLogger, IDslLogger<TCategoryName>
    {
        public DslLogger(ILogger<TCategoryName> coreLogger) 
            : base(coreLogger)
        {
        }
    }
}