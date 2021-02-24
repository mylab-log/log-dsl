using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

        private ILogger CreateCoreLogger()
        {
            var services = new ServiceCollection()
                .AddLogging(c => c
                    .AddProvider(new TestLoggerProvider(_output))
                    .SetMinimumLevel(LogLevel.Debug)
                )
                .BuildServiceProvider();

            return services.GetService<ILogger<DslLoggerBehavior>>();
        }

        private class TestLoggerProvider : ILoggerProvider
        {
            private readonly ITestOutputHelper _output;

            public TestLoggerProvider(ITestOutputHelper output)
            {
                _output = output;
            }

            public void Dispose()
            {
            }

            public ILogger CreateLogger(string categoryName)
            {
                return new TestLogger(_output);
            }
        }

        private class TestLogger : ILogger
        {
            private readonly ITestOutputHelper _output;

            public TestLogger(ITestOutputHelper output)
            {
                _output = output;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
                Func<TState, Exception, string> formatter)
            {
                _output.WriteLine(formatter(state, exception));
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                throw new NotImplementedException();
            }
        }
    }
}