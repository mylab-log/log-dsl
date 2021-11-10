using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace MyLab.Log.Dsl
{
    class DslLogger : IDslLogger
    {
        private readonly ILogger _coreLogger;
        private readonly IEnumerable<IDslLogContext> _contexts;

        public DslLogger(ILoggerFactory coreLoggerFactory, IEnumerable<IDslLogContext> contexts)
        {
            if (coreLoggerFactory == null) throw new ArgumentNullException(nameof(coreLoggerFactory));
            _contexts = contexts;

            _coreLogger = coreLoggerFactory.CreateLogger("");
        }

        public DslLogger(ILogger coreLogger, IEnumerable<IDslLogContext> contexts)
        {
            _coreLogger = coreLogger ?? throw new ArgumentNullException(nameof(coreLogger));
            _contexts = contexts;
        }

        public DslExpression Debug(string message)
        {
            CheckMessageForWhitespace(message);

            return new DslExpression(_coreLogger, message, _contexts, PredefinedLogLevels.Debug);
        }

        public DslExpression Action(string message)
        {
            CheckMessageForWhitespace(message);

            return new DslExpression(_coreLogger, message, _contexts);
        }

        public DslExpression Warning(string message)
        {
            CheckMessageForWhitespace(message);

            return new DslExpression(_coreLogger, message, _contexts, PredefinedLogLevels.Warning);
        }

        public DslExpression Error(string message)
        {
            CheckMessageForWhitespace(message);

            return new DslExpression(_coreLogger, message, _contexts, PredefinedLogLevels.Error);
        }

        void CheckMessageForWhitespace(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
                throw new ArgumentException("Value cannot be null or whitespace.", "val");
        }
    }

    class DslLogger<TCategoryName> : DslLogger, IDslLogger<TCategoryName>
    {
        public DslLogger(ILogger<TCategoryName> coreLogger, IEnumerable<IDslLogContext> contexts) 
            : base(coreLogger, contexts)
        {
        }
    }
}