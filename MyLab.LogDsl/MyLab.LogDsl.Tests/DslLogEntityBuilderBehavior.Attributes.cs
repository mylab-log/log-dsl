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

            var em = le.Attributes.FirstOrDefault(cc => cc.Name == AttributeNames.ExceptionMessage);
            var et = le.Attributes.FirstOrDefault(cc => cc.Name == AttributeNames.ExceptionType);
            var es = le.Attributes.FirstOrDefault(cc => cc.Name == AttributeNames.ExceptionStackTrace);

            Assert.NotNull(em);
            Assert.Equal(em.Value, ex.Message);

            Assert.NotNull(et);
            Assert.Equal(et.Value, ex.GetType().FullName);

            Assert.NotNull(es);
            Assert.Equal(es.Value, ex.StackTrace);
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

            var em = le.Attributes.FirstOrDefault(cc => cc.Name == AttributeNames.BaseExceptionMessage);
            var et = le.Attributes.FirstOrDefault(cc => cc.Name == AttributeNames.BaseExceptionType);
            var es = le.Attributes.FirstOrDefault(cc => cc.Name == AttributeNames.BaseExceptionStackTrace);

            Assert.NotNull(em);
            Assert.Equal(em.Value, bex.Message);

            Assert.NotNull(et);
            Assert.Equal(et.Value, bex.GetType().FullName);

            Assert.NotNull(es);
            Assert.Equal(es.Value, bex.StackTrace);
        }
    }
}