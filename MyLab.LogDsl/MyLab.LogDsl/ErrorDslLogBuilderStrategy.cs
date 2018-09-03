using Microsoft.Extensions.Logging;
using MyLab.Logging;

namespace MyLab.LogDsl
{
    class ErrorDslLogBuilderStrategy : IDslLogBuilderStrategy
    {
        public void WriteLogEntity(ILogger logger, LogEntity entity)
        {
            logger.Log(LogLevel.Error, entity.EventId, entity, null, LogEntityFormatter.Func);
        }
    }
}