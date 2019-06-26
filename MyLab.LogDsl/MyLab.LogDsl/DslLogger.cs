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
        internal static string CreateAggregatedExceptionAttrName(string attrName, int index) => attrName + "-aggr-" + index;
        
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
            {
                FillExceptionData(b, exception);
                
                if (exception is AggregateException aEx)
                {
                    b.AndMarkAs(Markers.AggregationException);

                    var exColl = aEx.InnerExceptions;
                    
                    if (exColl.Count != 1)
                    {
                        for (int i = 0; i < exColl.Count; i++)
                        {
                            FillExceptionData(b, exColl[i], i);
                        }
                    }
                }

                if (b.InstanceId == Guid.Empty)
                    b.AndMarkAs(Markers.ExceptionHasNoIdentifier);
            }
            
            return b;
        }

        void FillExceptionData(DslLogEntityBuilder b, Exception e, int index = -1)
        {
            b.AndFactIs(AttrNm(AttributeNames.ExceptionType), e.GetType().FullName)
                .AndFactIs(AttrNm(AttributeNames.ExceptionStackTrace), e.StackTrace)
                .AndFactIs(AttrNm(AttributeNames.ExceptionMessage), e.Message);
            var be = e.GetBaseException();

            if (be != null && be != e)
            {
                b.AndFactIs(AttrNm(AttributeNames.BaseExceptionType), be.GetType().FullName)
                    .AndFactIs(AttrNm(AttributeNames.BaseExceptionStackTrace), be.StackTrace)
                    .AndFactIs(AttrNm(AttributeNames.BaseExceptionMessage), be.Message)
                    .AndMarkAs(Markers.HasInnerException);
            }

            if (b.InstanceId == Guid.Empty)
            {
                var detectedId = GetIdDeep(e);
                if(detectedId != null)
                    b.InstanceId = detectedId.Value;
            }

            foreach (var marker in e.GetMarkers())
                b.AndMarkAs(marker);

            foreach (var condition in GetConditionsDeep(e))
                b.AndFactIs(condition.Key, condition.Value);
            
            string AttrNm(string attrName) => index < 0 
                ? attrName 
                : CreateAggregatedExceptionAttrName(attrName, index);

            IEnumerable<ExceptionCondition> GetConditionsDeep(Exception e1)
            {
                return e1.InnerException != null
                    ? e1.GetConditions().Union(GetConditionsDeep(e1.InnerException))
                    : e1.GetConditions();
            }

            Guid? GetIdDeep(Exception e2)
            {
                if (e2.HasId())
                    return e2.GetId();

                if (e2.InnerException != null)
                    return GetIdDeep(e2.InnerException);

                return null;
            } 
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