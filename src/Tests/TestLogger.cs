using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using MyLab.Log;

namespace Tests
{
    class TestLoggerProvider : ILoggerProvider
    {
        private readonly ILogger _instance;

        public TestLoggerProvider(ILogger instance)
        {
            _instance = instance;
        }

        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _instance;
        }
    }

    class TestLogger : ILogger
    {
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
