using System;
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
        [Obsolete("Use DI instead: services.AddDslLog() and DI with IDslLogger<TCat>", true)]
        public static IDslLogger Dsl(this ILogger coreLogger)
        {
            throw new NotSupportedException();
        }
    }
}
