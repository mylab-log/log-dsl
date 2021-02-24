using System;
using Microsoft.Extensions.Logging;

namespace MyLab.Log.Dsl
{
    class DslLogger : IDslLogger
    {
        public DslLogger(ILogger innerLogger = null)
        {
            if(innerLogger == null)
                throw new InvalidOperationException("Built-In .NET core logging required");
        }
    }

    class DslLogger<TCategory> : DslLogger, IDslLogger<TCategory>
    {
        public DslLogger(ILogger<TCategory> innerLogger = null) : base(innerLogger)
        {
            
        }
    }
}