using Microsoft.Extensions.Logging;
using MyLab.Logging;

namespace MyLab.LogDsl
{
    interface IDslLogBuilderStrategy
    {
        void WriteLogEntity(ILogger logger, LogEntity entity);
    }
}