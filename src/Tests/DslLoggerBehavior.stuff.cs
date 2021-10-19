using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyLab.Log.Dsl;
using Xunit.Abstractions;

namespace Tests
{
    public partial class DslLoggerBehavior
    {
        private readonly ITestOutputHelper _output;

        public DslLoggerBehavior(ITestOutputHelper output)
        {
            _output = output;
        }

        private IDslLogger CreateCoreLogger()
        {
            var services = new ServiceCollection()
                .AddLogging(c => c
                    .AddDsl()
                    .AddXUnit(_output)
                    .SetMinimumLevel(LogLevel.Debug)
                )
                .BuildServiceProvider();

            return services.GetService<IDslLogger<DslLoggerBehavior>>();
        }
    }
}