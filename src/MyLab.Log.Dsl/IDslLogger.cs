using System;

namespace MyLab.Log.Dsl
{
    /// <summary>
    /// Specifies DSL-style logger
    /// </summary>
    public interface IDslLogger
    {
        /// <summary>
        /// Begin DSL expression for debug log-event
        /// </summary>
        DslExpression Debug(string message);
        /// <summary>
        /// Begin DSL expression for action log-event
        /// </summary>
        DslExpression Action(string message);
        /// <summary>
        /// Begin DSL expression for warning log-event
        /// </summary>
        DslExpression Warning(string message);
        /// <summary>
        /// Begin DSL expression for error log-event
        /// </summary>
        DslExpression Error(string message);
    }
}
