using System;
using System.Linq;
using MyLab.Logging;
using Xunit;

namespace MyLab.LogDsl.Tests
{
    public partial class DslLogEntityBuilderBehavior
    {
        [Fact]
        public void ShouldAddStringCondition()
        {
            //Arrange
            LogEntity le = null;

            var logger = Tools.GetLogger((level, id, logEntity, e, formatter) => { le = logEntity; });

            //Act
            logger.Act("some thing")
                .AndFactIs("foo")
                .Write();

            //Assert

            var conditions = (string)le.Attributes
                .Find(a => a.Name == LogAttributeNames.ConditionsAttributeName)
                .Value;

            Assert.Contains("foo", conditions);
        }

        [Fact]
        public void ShouldAddExpressionCondition()
        {
            //Arrange
            int val = 50;
            LogEntity le = null;

            var logger = Tools.GetLogger((level, id, logEntity, e, formatter) => { le = logEntity; });

            //Act
            logger.Act("some thing").AndFactIs(() => val != 20).Write();

            //Assert
            Assert.NotNull(le);

            var conditions = (string)le.Attributes
                .Find(a => a.Name == LogAttributeNames.ConditionsAttributeName)
                .Value;

            Assert.Contains("'val != 20' is True", conditions);
        }

        [Fact]
        public void ShouldAddCustomCondition()
        {
            //Arrange
            LogEntity le = null;
            object value = new object();

            var logger = Tools.GetLogger((level, id, logEntity, e, formatter) => { le = logEntity; });

            //Act
            logger.Act("some thing").AndFactIs("name", value).Write();
            var cc = le.Attributes.FirstOrDefault(c => c.Name == "name");

            //Assert
            Assert.NotNull(cc);
            Assert.Equal(value, cc.Value);
        }

        [Fact]
        public void ShouldAddExceptionData()
        {
            //Arrange
            LogEntity le = null;
            
            var logger = Tools.GetLogger((level, id, logEntity, e, formatter) => { le = logEntity; });

            var ex = new Exception("foo");

            //Act
            logger.Error(ex).Write();

            //Assert
            Assert.NotNull(le);

            var a = le.Attributes.FirstOrDefault(cc => cc.Name == AttributeNames.Exception);

            var exDto = a?.Value as ExceptionDto;

            Assert.NotNull(exDto);
            Assert.Equal(ex.Message, exDto.Message);
            Assert.Equal(ex.GetType().FullName, exDto.Type);
            Assert.Equal(ex.StackTrace, exDto.StackTrace);
        }

        [Fact]
        public void ShouldAddBaseExceptionData()
        {
            //Arrange
            LogEntity le = null;

            var logger = Tools.GetLogger((level, id, logEntity, e, formatter) => { le = logEntity; });

            var bex = new Exception("bar");
            var ex = new Exception("foo", bex);

            //Act
            logger.Error(ex).Write();

            //Assert
            Assert.NotNull(le);

            var a = le.Attributes.FirstOrDefault(cc => cc.Name == AttributeNames.Exception);

            var exDto = a?.Value as ExceptionDto;

            Assert.NotNull(exDto);
            Assert.Equal(ex.Message, exDto.Message);
            Assert.Equal(ex.GetType().FullName, exDto.Type);
            Assert.Equal(ex.StackTrace, exDto.StackTrace);
        }
    }
}