using System;
using Microsoft.Extensions.Logging;
using MyLab.Logging;

namespace MyLab.LogDsl
{
    class ErrorDslLogBuilderStrategy : IDslLogBuilderStrategy
    {
        private readonly Exception _originException;

        public ErrorDslLogBuilderStrategy(Exception originException)
        {
            _originException = originException;
        }

        public ErrorDslLogBuilderStrategy()
        {
            
        }

        public void WriteLogEntity(ILogger logger, LogEntity entity)
        {
            logger.Log(LogLevel.Error, entity.EventId, entity, _originException, LogEntityFormatter.Func);
        }
    }
}