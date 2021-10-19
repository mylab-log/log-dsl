using System;
using Microsoft.Extensions.Logging;
using MyLab.Log;
using Xunit.Abstractions;

namespace Tests
{
    public partial class DslLoggerInjectionBehavior
    {
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// Initializes a new instance of <see cref="DslLoggerInjectionBehavior"/>
        /// </summary>
        public DslLoggerInjectionBehavior(ITestOutputHelper output)
        {
            _output = output;
        }

        private class TestLoggerProvider : ILoggerProvider
        {
            public void Dispose()
            {
            }

            public ILogger CreateLogger(string categoryName)
            {
                return TestLogger.Instance;
            }
        }

        private class TestLogger : ILogger
        {
            public static readonly TestLogger Instance = new TestLogger();

            public LogEntity LastMessage { get; set; }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception,
                Func<TState, Exception, string> formatter)
            {
                LastMessage = state as LogEntity;
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