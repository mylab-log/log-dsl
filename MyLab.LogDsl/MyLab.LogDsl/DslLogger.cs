using System;
using Microsoft.Extensions.Logging;
using MyLab.Logging;

namespace MyLab.LogDsl
{
    /// <summary>
    /// Provides abilities for DSL using when writing a log
    /// </summary>
    public class DslLogger
    {
        /// <summary>
        /// Get original logger
        /// </summary>
        public ILogger OriginLogger { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="DslLogger"/>
        /// </summary>
        public DslLogger(ILogger originLogger)
        {
            OriginLogger = originLogger;
        }

        /// <summary>
        /// Creates act log entity 
        /// </summary>
        public DslLogEntityBuilder Act(EventId eventId)
        {
            return new DslLogEntityBuilder(OriginLogger, new ActDslLogBuilderStrategy())
            {
                EventId = eventId.Id,
                Message = eventId.Name
            };
        }

        /// <summary>
        /// Creates error log entity 
        /// </summary>
        public DslLogEntityBuilder Error(EventId eventId, Exception exception = null)
        {
            var b = new DslLogEntityBuilder(OriginLogger, new ErrorDslLogBuilderStrategy())
            {
                EventId = eventId.Id,
                Message = eventId.Name
            }.AndMarkAs(Markers.Error);

            if (exception != null)
            {
                b = b.AndFactIs(AttributeNames.ExceptionType, exception.GetType().FullName)
                     .AndFactIs(AttributeNames.ExceptionStackTrace, exception.StackTrace)
                     .AndFactIs(AttributeNames.ExceptionMessage, exception.Message);
                var be = exception.GetBaseException();

                if (be != null && be != exception)
                {
                    b = b.AndFactIs(AttributeNames.BaseExceptionType, be.GetType().FullName)
                        .AndFactIs(AttributeNames.BaseExceptionStackTrace, be.StackTrace)
                        .AndFactIs(AttributeNames.BaseExceptionMessage, be.Message);
                }
            }

            return b;
        }

        /// <summary>
        /// Creates debug log entity 
        /// </summary>
        public DslLogEntityBuilder Debug(EventId eventId)
        {
            var b = new DslLogEntityBuilder(OriginLogger, new DebugDslLogBuilderStrategy())
            {
                EventId = eventId.Id,
                Message = eventId.Name
            }.AndMarkAs(Markers.Debug);

            return b;
        }
    }
}