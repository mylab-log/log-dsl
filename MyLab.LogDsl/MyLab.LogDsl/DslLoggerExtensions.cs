using System;
using Microsoft.Extensions.Logging;

namespace MyLab.LogDsl
{
    /// <summary>
    /// Extension for <see cref="DslLogger"/>
    /// </summary>
    public static class DslLoggerExtensions
    {
        /// <summary>
        /// Creates act log entity 
        /// </summary>
        public static DslLogEntityBuilder Act(this DslLogger logger, string msg)
        {
            return logger.Act(new EventId(0, msg));
        }

        /// <summary>
        /// Creates act log entity 
        /// </summary>
        public static DslLogEntityBuilder Act(this DslLogger logger, int eventId)
        {
            return logger.Act(new EventId(eventId));
        }

        /// <summary>
        /// Creates error log entity 
        /// </summary>
        public static DslLogEntityBuilder Error(this DslLogger logger, Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            return logger.Error(new EventId(0, exception.Message), exception);
        }

        /// <summary>
        /// Creates error log entity 
        /// </summary>
        public static DslLogEntityBuilder Error(this DslLogger logger, string msg, Exception exception = null)
        {
            return logger.Error(new EventId(0, msg), exception);
        }

        /// <summary>
        /// Creates error log entity 
        /// </summary>
        public static DslLogEntityBuilder Error(this DslLogger logger, int eventId, Exception exception = null)
        {
            return logger.Error(new EventId(eventId), exception);
        }
    }
}