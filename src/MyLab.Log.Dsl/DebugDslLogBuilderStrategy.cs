using Microsoft.Extensions.Logging;
using MyLab.Logging;

namespace MyLab.LogDsl
{
    class DebugDslLogBuilderStrategy : IDslLogBuilderStrategy
    {
        public void WriteLogEntity(ILogger logger, LogEntity entity)
        {
            logger.Log(LogLevel.Debug, entity.EventId, entity, null, LogEntityFormatter.Func);
        }
    }
}