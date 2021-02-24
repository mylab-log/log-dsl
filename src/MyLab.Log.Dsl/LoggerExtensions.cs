using Microsoft.Extensions.Logging;

namespace MyLab.Log.Dsl
{
    /// <summary>
    /// Contains extensions log methods for <see cref="ILogger"/>
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Create DSL-logger for core logger
        /// </summary>
        public static IDslLogger Dsl(this ILogger coreLogger)
        {
            return new DslLogger(coreLogger);
        }
    }
}
