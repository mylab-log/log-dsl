using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using MyLab.Logging;

namespace MyLab.LogDsl.Tests
{
    static class Tools
    {
        public static DslLogger GetLogger(Action<LogLevel, EventId, LogEntity, Exception, Func<LogEntity, Exception, string>> loggerAction)
        {
            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(l =>
                    l.Log(
                        It.IsAny<LogLevel>(),
                        It.IsAny<EventId>(),
                        It.IsAny<LogEntity>(),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<LogEntity, Exception, string>>()))
                .Callback(loggerAction);

            var loggerProvider = Mock.Of<ILoggerProvider>(p =>
                p.CreateLogger(It.IsAny<string>()) == loggerMock.Object);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(c => c.AddProvider(loggerProvider));

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var lf = serviceProvider.GetService<ILoggerFactory>();

            return lf.CreateLogger("MyCategory").Dsl();
        }
    }
}
