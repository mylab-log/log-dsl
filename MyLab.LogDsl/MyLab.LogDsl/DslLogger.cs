using System;
using System.Collections.Generic;
using System.Linq;
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
        readonly ILogger _originLogger;

        private readonly ILogDataSource _initialDataSource;
        
        /// <summary>
        /// Initializes a new instance of <see cref="DslLogger"/>
        /// </summary>
        public DslLogger(ILogger originLogger, ILogDataSource initialDataSource = null)
        {
            _originLogger = originLogger;
            _initialDataSource = initialDataSource;
        }

        /// <summary>
        /// Creates act log entity 
        /// </summary>
        public DslLogEntityBuilder Act(EventId eventId)
        {
            var lb = new DslLogEntityBuilder(_originLogger, new ActDslLogBuilderStrategy())
            {
                EventId = eventId.Id,
                Message = eventId.Name
            };

            _initialDataSource?.AddLogData(lb);

            return lb;
        }

        /// <summary>
        /// Creates error log entity 
        /// </summary>
        public DslLogEntityBuilder Error(EventId eventId, Exception exception = null)
        {
            return CreateErrorBased(eventId, exception, Markers.Error, new ErrorDslLogBuilderStrategy(exception));
        }

        /// <summary>
        /// Creates warning log entity 
        /// </summary>
        public DslLogEntityBuilder Warning(EventId eventId, Exception exception = null)
        {
            return CreateErrorBased(eventId, exception, Markers.Warning, new WarningDslLogBuilderStrategy(exception));
        }

        /// <summary>
        /// Creates debug log entity 
        /// </summary>
        public DslLogEntityBuilder Debug(EventId eventId)
        {
            var lb = new DslLogEntityBuilder(_originLogger, new DebugDslLogBuilderStrategy())
            {
                EventId = eventId.Id,
                Message = eventId.Name
            }.AndMarkAs(Markers.Debug);

            _initialDataSource?.AddLogData(lb);

            return lb;
        }

        DslLogEntityBuilder CreateErrorBased(
            EventId eventId, 
            Exception exception, 
            string marker,
            IDslLogBuilderStrategy strategy)
        {
            var b = new DslLogEntityBuilder(_originLogger, strategy)
            {
                EventId = eventId.Id,
                Message = eventId.Name
            }.AndMarkAs(marker);

            if (exception != null)
                b.AndFactIs(
                    AttributeNames.Exception,
                    ExceptionDto.Create(exception));

            _initialDataSource?.AddLogData(b);

            return b;
        }
    }
}