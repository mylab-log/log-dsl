using System;
using MyLab.Logging;
using Xunit;

namespace MyLab.LogDsl.Tests
{
    public class DslLoggerBehavior
    {
        [Fact]
        public void ShouldUseExceptionId()
        {
            //Arrange
            var ex = new Exception();
            Guid logInstanceId = Guid.Empty;

            var logger = Tools.GetLogger((level, id, logEntity, e, formatter) => { logInstanceId = logEntity.Id; });

            //Act
            logger.Error(ex).Write();

            //Assert
            Assert.NotEqual(Guid.Empty, logInstanceId);
            Assert.Equal(ex.GetId(), logInstanceId);
        }

        [Fact]
        public void ShouldUseExceptionConditions()
        {
            //Arrange
            var ex = new Exception();
            LogEntity le = null;

            ex.AndFactIs("foo", "bar");

            var logger = Tools.GetLogger((level, id, logEntity, e, formatter) => { le = logEntity; });

            //Act
            logger.Error(ex).Write();

            //Assert
            Assert.NotNull(le);
            Assert.Contains(le.Attributes, ca => ca.Name == "foo" && (string) ca.Value == "bar");
        }

        [Fact]
        public void ShouldUseExceptionMarkers()
        {
            //Arrange
            var ex = new Exception();
            LogEntity le = null;

            ex.AndMarkAs("foo");

            var logger = Tools.GetLogger((level, id, logEntity, e, formatter) => { le = logEntity; });

            //Act
            logger.Error(ex).Write();

            //Assert
            Assert.NotNull(le);
            Assert.Contains(le.Markers, m => m == "foo");
        }
    }
}
