using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using MyLab.Logging;

namespace MyLab.LogDsl
{
    /// <summary>
    /// Build log entity
    /// </summary>
    public class DslLogEntityBuilder
    {
        private readonly ILogger _logger;
        private readonly IDslLogBuilderStrategy _strategy;

        readonly LogEntity _entity;

        /// <summary>
        /// Event identifier
        /// </summary>
        public int EventId
        {
            get => _entity.EventId;
            set => _entity.EventId = value;
        }

        /// <summary>
        /// Event message
        /// </summary>
        public string Message
        {
            get => _entity.Message;
            set => _entity.Message = value;
        }

        internal DslLogEntityBuilder(ILogger logger, IDslLogBuilderStrategy strategy)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));

            _entity = new LogEntity();
        }

        private DslLogEntityBuilder(DslLogEntityBuilder origin)
        {
            _logger = origin._logger;
            _strategy = origin._strategy;
            _entity = origin._entity.Clone();
        }

        /// <summary>
        /// Writes log entity to logger
        /// </summary>
        public void Write(Guid? logInstanceId = null)
        {
            var e = _entity.Clone(logInstanceId ?? Guid.NewGuid());
            _strategy.WriteLogEntity(_logger, e);
        }

        /// <summary>
        /// Adds marker for log entity
        /// </summary>
        public DslLogEntityBuilder AndMarkAs(string marker)
        {
            var clone = new DslLogEntityBuilder(this);
            clone._entity.Markers.Add(marker);
            return clone;
        }

        /// <summary>
        /// Specifies condition at that moment
        /// </summary>
        public DslLogEntityBuilder AndFactIs(string condition)
        {
            if (string.IsNullOrWhiteSpace(condition))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(condition));

            var clone = new DslLogEntityBuilder(this);

            clone._entity.Conditions.Add(condition);

            return clone;
        }

        /// <summary>
        /// Specifies condition at that moment
        /// </summary>
        public DslLogEntityBuilder AndFactIs(string key, object value)
        {
            var clone = new DslLogEntityBuilder(this);

            clone._entity.CustomConditions.Add(new LogEntityCustomCondition(key, value));

            return clone;
        }

        /// <summary>
        /// Specifies condition at that moment
        /// </summary>
        public DslLogEntityBuilder AndFactIs(Expression<Func<bool>> condition)
        {
            string conditionDescription = ExpressionToString(condition);

            var clone = new DslLogEntityBuilder(this);

            clone._entity.Conditions.Add(conditionDescription);

            return clone;
        }

        static string ExpressionToString(Expression<Func<bool>> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            string conditionState;

            try
            {
                conditionState = expression.Compile()().ToString();
            }
            catch (Exception e)
            {
                conditionState = $"[Exception has thrown: '{e.GetBaseException().Message}']";
            }

            return $"'{ExpressionToOriginalString(expression)}' is {conditionState}";
        }

        static string ExpressionToOriginalString(Expression expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            string expressionString = expression.ToString();
            string originalString = Regex.Replace(expressionString,
                @"(^\(\) \=\> \()|(value\([a-zA-Z0-9\.]+\+\<\>c__DisplayClass[\d_]+\)\.)|(\)$)", 
                String.Empty);

            return originalString;
        }
    }
}