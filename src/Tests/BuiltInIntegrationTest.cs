using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyLab.Log.Dsl;
using Xunit;

namespace Tests
{
    public class BuiltInIntegrationTest
    {
        [Fact]
        public void ShouldProvideLogger()
        {
            //Arrange
            var sp = new ServiceCollection()
                .AddLogging()
                .AddSingleton<ILogger, TestLogger>()
                .AddDslLog()
                .BuildServiceProvider();
            
            //Act
            var logger = sp.GetService<IDslLogger>();

            //Assert
            Assert.NotNull(logger);
        }

        [Fact]
        public void ShouldProvideLoggerWithCategory()
        {
            //Arrange
            var sp = new ServiceCollection()
                .AddLogging()
                .AddDslLog()
                .BuildServiceProvider();

            //Act
            var logger = sp.GetService<IDslLogger<BuiltInIntegrationTest>>();

            //Assert
            Assert.NotNull(logger);
        }

        [Fact]
        public void ShouldFailIfLoggingIsAbsent()
        {
            //Arrange
            var sp = new ServiceCollection()
                //.AddLogging()
                .AddDslLog()
                .BuildServiceProvider();

            //Act & Assert
            Assert.Throws<InvalidOperationException>(() => sp.GetService<IDslLogger<BuiltInIntegrationTest>>());
        }

        class TestLogger : ILogger
        {
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                throw new NotImplementedException();
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                throw new NotImplementedException();
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                throw new NotImplementedException();
            }
        }
    }
}
