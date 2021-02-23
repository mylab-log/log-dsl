using MyLab.Logging;
using Xunit;

namespace MyLab.LogDsl.Tests
{
    public partial class DslLogEntityBuilderBehavior
    {
        [Fact]
        public void ShouldAddStringEventId()
        {
            //Arrange
            LogEntity le = null;

            var logger = Tools.GetLogger((level, id, logEntity, e, formatter) => { le = logEntity; });

            //Act
            logger.Act("foo").Write();

            //Assert
            Assert.Equal(0, le.EventId);
            Assert.Equal("foo", le.Content);
        }

        [Fact]
        public void ShouldAddIntEventId()
        {
            //Arrange
            LogEntity le = null;

            var logger = Tools.GetLogger((level, id, logEntity, e, formatter) => { le = logEntity; });

            //Act
            logger.Act(123).Write();

            //Assert
            Assert.Equal(123, le.EventId);
            Assert.Null(le.Content);
        }
    }
}