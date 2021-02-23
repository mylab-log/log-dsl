using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using MyLab.Logging;
using Xunit;

namespace MyLab.LogDsl.Tests
{
    public partial class DslLogEntityBuilderBehavior
    {
        [Fact]
        public void ShouldAddMarkers()
        {
            //Arrange
            LogEntity le = null;

            var logger = Tools.GetLogger((level, id, logEntity, e, formatter) =>
            {
                le = logEntity;
            });

            //Act
            logger.Act("some thing").AndMarkAs("foo").Write();

            //Assert
            Assert.Contains(le.Markers, s => s == "foo");
        }

        [Fact]
        public void ShouldAddOriginExceptionIntoLogger()
        {
            //Arrange
            var originException = new Exception();
            Exception gotException = null; 

            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(p => p.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<LogEntity>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<LogEntity, Exception, string>>()
                ))
                .Callback((Action<LogLevel, EventId, LogEntity, Exception, Func<LogEntity, Exception, string>>)
                    ((ll, ei, le, ex, f) => gotException = ex));

            //Act
            loggerMock.Object.Dsl().Error(originException).Write();

            //Assert
            Assert.Equal(originException, gotException);
        }
    }
}
