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

        /// <summary>
        /// Initializes a new instance of <see cref="DslLogger"/>
        /// </summary>
        public DslLogger(ILogger originLogger)
        {
            _originLogger = originLogger;
        }

        /// <summary>
        /// Creates act log entity 
        /// </summary>
        public DslLogEntityBuilder Act(EventId eventId)
        {
            return new DslLogEntityBuilder(_originLogger, new ActDslLogBuilderStrategy())
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
            var b = new DslLogEntityBuilder(_originLogger, new ErrorDslLogBuilderStrategy(exception))
            {
                EventId = eventId.Id,
                Message = eventId.Name
            }.AndMarkAs(Markers.Error);

            if (exception != null)
                b.AndFactIs(
                    AttributeNames.Exception, 
                    ExceptionDto.Create(exception));
            
            return b;
        }

        /// <summary>
        /// Creates debug log entity 
        /// </summary>
        public DslLogEntityBuilder Debug(EventId eventId)
        {
            return new DslLogEntityBuilder(_originLogger, new DebugDslLogBuilderStrategy())
            {
                EventId = eventId.Id,
                Message = eventId.Name
            }.AndMarkAs(Markers.Debug);
        }
    }
}