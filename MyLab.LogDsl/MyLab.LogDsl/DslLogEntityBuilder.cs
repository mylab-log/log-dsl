using System;
using System.Collections.Generic;
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

        private readonly List<LogEntityAttribute> _attributes  = new List<LogEntityAttribute>();
        private readonly List<string> _markers  = new List<string>();
        private readonly List<string> _conditions  = new List<string>();
        
        /// <summary>
        /// Event identifier
        /// </summary>
        public Guid InstanceId { get; set; }

        /// <summary>
        /// Event identifier
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Event message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Event time
        /// </summary>
        public DateTime Time { get; set; } = DateTime.Now;

        internal DslLogEntityBuilder(ILogger logger, IDslLogBuilderStrategy strategy)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _strategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        }

        /// <summary>
        /// Writes log entity to logger
        /// </summary>
        public void Write(Guid? logInstanceId = null)
        {
            Guid resId;
                
            if(logInstanceId != null)
                resId = logInstanceId.Value;
            else
            {
                resId = InstanceId != Guid.Empty
                    ? InstanceId
                    : Guid.NewGuid();
            }

            var le = new LogEntity
            {
                Id = resId,
                Time = Time,
                EventId = EventId,
                Content = Message
            };

            if(_markers.Count != 0)
                le.Markers = new List<string>(_markers);
            if(_attributes.Count != 0)
                le.Attributes = new List<LogEntityAttribute>(_attributes);
            if (_conditions.Count != 0)
            {
                var condAttr = new ConditionsLogEntityAttribute(_conditions);

                if (_attributes.Count != 0)
                    le.Attributes.Add(condAttr);
                else
                    le.Attributes = new List<LogEntityAttribute> {condAttr};
            }

            _strategy.WriteLogEntity(_logger, le);
        }

        /// <summary>
        /// Adds marker for log entity
        /// </summary>
        public DslLogEntityBuilder AndMarkAs(string marker)
        {
            _markers.Add(marker);
            return this;
        }

        /// <summary>
        /// Specifies condition at that moment
        /// </summary>
        public DslLogEntityBuilder AndFactIs(string condition)
        {
            if (string.IsNullOrWhiteSpace(condition))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(condition));
            _conditions.Add(condition);

            return this;
        }

        /// <summary>
        /// Specifies condition at that moment
        /// </summary>
        public DslLogEntityBuilder AndFactIs(string name, object value)
        {
            _attributes.Add(new LogEntityAttribute(name, value));

            return this;
        }

        /// <summary>
        /// Specifies condition at that moment
        /// </summary>
        public DslLogEntityBuilder AndFactIs(Expression<Func<bool>> condition)
        {
            string conditionDescription = ExpressionToString(condition);

            _conditions.Add(conditionDescription);

            return this;
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