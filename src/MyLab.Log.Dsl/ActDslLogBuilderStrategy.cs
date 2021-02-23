using Microsoft.Extensions.Logging;
using MyLab.Logging;

namespace MyLab.LogDsl
{
    class ActDslLogBuilderStrategy : IDslLogBuilderStrategy
    {
        public void WriteLogEntity(ILogger logger, LogEntity entity)
        {
            logger.Log(LogLevel.Information,entity.EventId, entity, null, LogEntityFormatter.Func);
        }
    }
}