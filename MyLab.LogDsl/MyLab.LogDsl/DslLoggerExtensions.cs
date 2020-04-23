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
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            return logger.Act(new EventId(0, msg));
        }

        /// <summary>
        /// Creates act log entity 
        /// </summary>
        public static DslLogEntityBuilder Act(this DslLogger logger, int eventId)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            return logger.Act(new EventId(eventId));
        }

        /// <summary>
        /// Creates act log entity 
        /// </summary>
        public static DslLogEntityBuilder Debug(this DslLogger logger, string msg)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            return logger.Debug(new EventId(0, msg));
        }

        /// <summary>
        /// Creates act log entity 
        /// </summary>
        public static DslLogEntityBuilder Debug(this DslLogger logger, int eventId)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            return logger.Debug(new EventId(eventId));
        }

        /// <summary>
        /// Creates error log entity 
        /// </summary>
        public static DslLogEntityBuilder Error(this DslLogger logger, Exception exception)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            return logger.Error(new EventId(0, exception.Message), exception);
        }

        /// <summary>
        /// Creates error log entity 
        /// </summary>
        public static DslLogEntityBuilder Error(this DslLogger logger, string msg, Exception exception = null)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            return logger.Error(new EventId(0, msg), exception);
        }

        /// <summary>
        /// Creates error log entity 
        /// </summary>
        public static DslLogEntityBuilder Error(this DslLogger logger, int eventId, Exception exception = null)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            return logger.Error(new EventId(eventId), exception);
        }

        /// <summary>
        /// Creates warning log entity 
        /// </summary>
        public static DslLogEntityBuilder Warning(this DslLogger logger, Exception exception)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            return logger.Warning(new EventId(0, exception.Message), exception);
        }

        /// <summary>
        /// Creates warning log entity 
        /// </summary>
        public static DslLogEntityBuilder Warning(this DslLogger logger, string msg, Exception exception = null)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            return logger.Warning(new EventId(0, msg), exception);
        }

        /// <summary>
        /// Creates warning log entity 
        /// </summary>
        public static DslLogEntityBuilder Warning(this DslLogger logger, int eventId, Exception exception = null)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            return logger.Warning(new EventId(eventId), exception);
        }
    }
}