using System;
using Microsoft.Extensions.Logging;
using MyLab.Logging;

namespace MyLab.LogDsl
{
    class WarningDslLogBuilderStrategy : IDslLogBuilderStrategy
    {
        private readonly Exception _originException;

        public WarningDslLogBuilderStrategy(Exception originException)
        {
            _originException = originException;
        }

        public WarningDslLogBuilderStrategy()
        {

        }

        public void WriteLogEntity(ILogger logger, LogEntity entity)
        {
            logger.Log(LogLevel.Warning, entity.EventId, entity, _originException, LogEntityFormatter.Func);
        }
    }
}