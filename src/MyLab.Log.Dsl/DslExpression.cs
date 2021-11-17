using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace MyLab.Log.Dsl
{
    /// <summary>
    /// Build DSL expression which describes log entity
    /// </summary>
    public class DslExpression
    {
        private readonly ILogger _coreLogger;

        /// <summary>
        /// Log message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Log level literal identifier
        /// </summary>
        public string LogLevelLabel { get; }

        /// <summary>
        /// Gets reason exception
        /// </summary>
        public Exception ReasonException { get; private set; }

        /// <summary>
        /// Gets collection of event conditions
        /// </summary>
        public IReadOnlyCollection<string> Conditions { get; private set; } = Array.Empty<string>();

        /// <summary>
        /// Gets collection of log event labels
        /// </summary>
        public IReadOnlyDictionary<string, string> Labels{ get; private set; } = new Dictionary<string, string>();

        /// <summary>
        /// Gets collection of log event facts
        /// </summary>
        public IReadOnlyDictionary<string, object> Facts{ get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of <see cref="DslExpression"/>
        /// </summary>
        public DslExpression(ILogger coreLogger, string message, string logLevelLabel = null)
        {
            _coreLogger = coreLogger;
            Message = message;
            LogLevelLabel = logLevelLabel;
        }

        /// <summary>
        /// Apply exception as log event reason
        /// </summary>
        public DslExpression BecauseOf(Exception reasonException)
        {
            var clone = Clone();
            
            clone.ReasonException = reasonException;

            return clone;
        }

        /// <summary>
        /// Specifies condition at that moment
        /// </summary>
        public DslExpression AndFactIs(Expression<Func<bool>> condition)
        {
            return AndFactIs(ExpressionToString(condition));
        }

        /// <summary>
        /// Specifies condition at that moment
        /// </summary>
        public DslExpression AndFactIs(string condition)
        {
            if (string.IsNullOrWhiteSpace(condition))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(condition));

            var clone = Clone();
            
            clone.Conditions = new List<string>(Conditions){ condition }.AsReadOnly();

            return clone;
        }

        /// <summary>
        /// Specifies condition at that moment
        /// </summary>
        public DslExpression AndFactIs(string name, object value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var clone = Clone();

            clone.Facts = new ReadOnlyDictionary<string, object>(
                new Dictionary<string, object>(Facts) {{name, value ?? "[null]"}}
                );

            return clone;
        }

        /// <summary>
        /// Marks event with flag - label (true)
        /// </summary>
        public DslExpression AndLabel(string flagLabelName)
        {
            return AndLabel(flagLabelName, "true");
        }

        /// <summary>
        /// Marks event with label
        /// </summary>
        public DslExpression AndLabel(string labelName, string labelValue)
        {
            if (labelName == null) throw new ArgumentNullException(nameof(labelName));

            var clone = Clone();

            clone.Labels = new ReadOnlyDictionary<string, string>(
                    new Dictionary<string, string>(Labels) {  { labelName, labelValue ?? "[null]" } }
                    );

            return clone;
        }

        /// <summary>
        /// Creates <see cref="LogEntity"/> with collected parameters
        /// </summary>
        public LogEntity Create()
        {
            return CreateCore();
        }

        /// <summary>
        /// Writes <see cref="LogEntity"/> with collected parameters into core logger
        /// </summary>
        public void Write()
        {
            _coreLogger.Log(GetLogLevel(), default, Create(), null, LogEntityFormatter.Yaml);
        }

        private LogLevel GetLogLevel()
        {
            switch (LogLevelLabel)
            {
                case PredefinedLogLevels.Debug: return LogLevel.Debug;
                case PredefinedLogLevels.Error: return LogLevel.Error;
                case PredefinedLogLevels.Warning: return LogLevel.Warning;
                default: return LogLevel.Information;
            }
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

        DslExpression Clone()
        {
            return new DslExpression(_coreLogger, Message, LogLevelLabel)
            {
                ReasonException = ReasonException,
                Facts = Facts,
                Conditions = Conditions,
                Labels = Labels
            };
        }

        LogEntity CreateCore()
        {
            var log = new LogEntity
            {
                Exception = ReasonException,
                Message = Message
            };

            foreach (var label in Labels)
                log.Labels.Add(label.Key, label.Value);

            foreach (var fact in Facts)
                log.Facts.Add(fact.Key, fact.Value);

            if (Conditions.Count != 0)
            {
                log.Facts.Add(PredefinedFacts.Conditions, Conditions.ToArray());
            }

            if (!string.IsNullOrWhiteSpace(LogLevelLabel))
            {
                log.Labels.Add(PredefinedLabels.LogLevel, LogLevelLabel);
            }

            return log;
        }
    }
}