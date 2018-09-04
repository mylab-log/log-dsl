using System;
using Xunit;

namespace MyLab.LogDsl.Tests
{
    public partial class DslLogEntityBuilderBehavior
    {
        [Fact]
        public void ShouldGenerateLogInstanceIdIfNotSpecified()
        {
            //Arrange
            Guid logInstanceId = Guid.Empty;

            var logger = Tools.GetLogger((level, id, logEntity, e, formatter) =>
            {
                logInstanceId = logEntity.InstanceId;
            });

            //Act
            logger.Act("some thing").Write();

            //Assert
            Assert.NotEqual(Guid.Empty, logInstanceId);
        }

        [Fact]
        public void ShouldNotGenerateLogInstanceIdIfSpecified()
        {
            //Arrange
            Guid specified = Guid.NewGuid();
            Guid logInstanceId = Guid.Empty;

            var logger = Tools.GetLogger((level, id, logEntity, e, formatter) => { logInstanceId = logEntity.InstanceId; });

            //Act
            logger.Act("some thing").Write(specified);

            //Assert
            Assert.Equal(specified, logInstanceId);
        }

    }
}