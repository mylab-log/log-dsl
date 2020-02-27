using System;
using System.Collections.Generic;
using System.Linq;
using MyLab.Logging;
using Xunit;

namespace MyLab.LogDsl.Tests
{
    public class DslLoggerBehavior
    {
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
            Assert.Contains(le.Attributes, ca =>
            {
                return ca.Name == AttributeNames.Exception 
                       && ca.Value is ExceptionDto exceptionDto 
                       && exceptionDto.Conditions.Any(ca2 => (string)ca2.Value == "bar");
            });
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
            Assert.Contains(le.Attributes, ca =>
            {
                return ca.Name == AttributeNames.Exception
                       && ca.Value is ExceptionDto exceptionDto
                       && exceptionDto.Markers.Contains("foo");
            });
        }

        [Fact]
        public void ShouldDetectAggregationException()
        {
            //Arrange
            var e1 = new Exception("foo");
            var e2 = new Exception("bar");
            var ae = new AggregateException(e1, e2);

            LogEntityAttribute exceptionAttr = null;

            var l = Tools.GetLogger(
                (level, id, logEntity, e, formatter) =>
                {
                    exceptionAttr = logEntity.Attributes.FirstOrDefault(a => a.Name == AttributeNames.Exception);
                });
            
            //Act
            try
            {
                throw ae;
            }
            catch (Exception e)
            {
                l.Error(e).Write();
            }

            //Assert
            Assert.NotNull(exceptionAttr);
            Assert.NotNull(((ExceptionDto)exceptionAttr.Value).Aggregated);
        }
        
        [Fact]
        public void ShouldAddMarkerWhenDetectInnerExceptions()
        {
            //Arrange
            var eBase = new Exception("foo");
            var e = new Exception("bar", eBase);

            LogEntityAttribute exceptionAttr = null;

            var l = Tools.GetLogger(
                (level, id, logEntity, exx, formatter) =>
                {
                    exceptionAttr = logEntity.Attributes.FirstOrDefault(a => a.Name == AttributeNames.Exception);
                });

            //Act
            try
            {
                throw e;
            }
            catch (Exception ex)
            {
                l.Error(ex).Write();
            }

            //Assert
            Assert.NotNull(exceptionAttr);
            Assert.NotNull(((ExceptionDto)exceptionAttr.Value).Inner);
            Assert.Equal("foo", ((ExceptionDto)exceptionAttr.Value).Inner.Message);
        }
    }
}
