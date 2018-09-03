using Microsoft.Extensions.Logging;

namespace MyLab.LogDsl
{
    /// <summary>
    /// Extensions for <see cref="ILogger{TCategoryName}"/>
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Gets DSL logger for .NET Core logger
        /// </summary>
        public static DslLogger Dsl<TCategoryName>(this ILogger<TCategoryName> originLogger)
        {
            return new DslLogger(originLogger);
        }

        /// <summary>
        /// Gets DSL logger for .NET Core logger
        /// </summary>
        public static DslLogger Dsl(this ILogger originLogger)
        {
            return new DslLogger(originLogger);
        }
    }
}
