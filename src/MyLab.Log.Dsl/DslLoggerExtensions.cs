using System;

namespace MyLab.Log.Dsl
{
    /// <summary>
    /// Extensions for <see cref="IDslLogger"/>
    /// </summary>
    public static class DslLoggerExtensions
    {
        /// <summary>
        /// Begin DSL expression for warning log-event
        /// </summary>
        public static DslExpression Warning(this IDslLogger dslLogger, string message, Exception reasonException)
        {
            if (dslLogger == null) throw new ArgumentNullException(nameof(dslLogger));

            return dslLogger.Warning(message).BecauseOf(reasonException);
        }

        /// <summary>
        /// Begin DSL expression for error log-event
        /// </summary>
        public static DslExpression Error(this IDslLogger dslLogger, string message, Exception reasonException)
        {
            if (dslLogger == null) throw new ArgumentNullException(nameof(dslLogger));

            return dslLogger.Error(message).BecauseOf(reasonException);
        }

        /// <summary>
        /// Begin DSL expression for warning log-event
        /// </summary>
        public static DslExpression Warning(this IDslLogger dslLogger, Exception reasonException)
        {
            if (dslLogger == null) throw new ArgumentNullException(nameof(dslLogger));
            if (reasonException == null) throw new ArgumentNullException(nameof(reasonException));

            return dslLogger.Warning(reasonException.Message).BecauseOf(reasonException);
        }

        /// <summary>
        /// Begin DSL expression for error log-event
        /// </summary>
        public static DslExpression Error(this IDslLogger dslLogger, Exception reasonException)
        {
            if (dslLogger == null) throw new ArgumentNullException(nameof(dslLogger));
            if (reasonException == null) throw new ArgumentNullException(nameof(reasonException));

            return dslLogger.Error(reasonException.Message).BecauseOf(reasonException);
        }
    }
}