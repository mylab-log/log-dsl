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
        public void ShouldUseExceptionId()
        {
            //Arrange
            var ex = new Exception();
            ex.GetId();
            
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

        [Fact]
        public void ShouldDetectAggregationException()
        {
            //Arrange
            var e1 = new Exception("foo");
            var e2 = new Exception("bar");
            var ae = new AggregateException(e1, e2);

            List<string> leMarkers = null;

            var l = Tools.GetLogger((level, id, logEntity, e, formatter) => { leMarkers = logEntity.Markers; });
            
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
            Assert.Contains(leMarkers,s => s == Markers.AggregationException);
        }

        [Fact]
        public void ShouldIncludeAggregatedExceptions()
        {
            //Arrange
            var e1 = new Exception("foo");
            var e2 = new Exception("bar");
            var ae = new AggregateException(e1, e2);

            string detectedE1Msg = null;
            string detectedE2Msg = null;
            
            var l = Tools.GetLogger((level, id, logEntity, e, formatter) =>
            {
                detectedE1Msg = GetAggrExcMsg(logEntity, 0);
                detectedE2Msg = GetAggrExcMsg(logEntity, 1);
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
            Assert.Equal("foo", detectedE1Msg);
            Assert.Equal("bar", detectedE2Msg);
            
            string GetAggrExcMsg(LogEntity logEntity, int excIndex)
            {
                return logEntity.Attributes
                    .Where(a => a.Name ==
                                DslLogger.CreateAggregatedExceptionAttrName(AttributeNames.ExceptionMessage, excIndex))
                    .Select(a => a.Value.ToString())
                    .FirstOrDefault();   
            }
        }
        
        [Fact]
        public void ShouldIncludeBaseAggregatedExceptions()
        {
            //Arrange
            var e1Base = new Exception("fooBase");
            var e2Base = new Exception("barBase");
            var e1 = new Exception("foo", e1Base);
            var e2 = new Exception("bar", e2Base);
            var ae = new AggregateException(e1, e2);

            string detectedE1BaseMsg = null;
            string detectedE2BaseMsg = null;
            
            var l = Tools.GetLogger((level, id, logEntity, e, formatter) =>
            {
                detectedE1BaseMsg = GetAggrBaseExcMsg(logEntity, 0);
                detectedE2BaseMsg = GetAggrBaseExcMsg(logEntity, 1);
                detectedE2BaseMsg = GetAggrBaseExcMsg(logEntity, 1);
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
            Assert.Equal("fooBase", detectedE1BaseMsg);
            Assert.Equal("barBase", detectedE2BaseMsg);
            
            string GetAggrBaseExcMsg(LogEntity logEntity, int excIndex)
            {
                return logEntity.Attributes
                    .Where(a => a.Name ==
                                DslLogger.CreateAggregatedExceptionAttrName(AttributeNames.BaseExceptionMessage, excIndex))
                    .Select(a => a.Value.ToString())
                    .FirstOrDefault();   
            }
        }
        
        [Fact]
        public void ShouldAddConditionsFromAllExceptions()
        {
            //Arrange
            var eBase = new Exception("foo");
            var e = new Exception("bar", eBase);

            eBase.AndFactIs("foo-fact", "foo-fact-val");

            List<LogEntityAttribute> caughtAttributes = null;
            
            var l = Tools.GetLogger((level, id, logEntity, exception, formatter) =>
                {
                    caughtAttributes = logEntity.Attributes;
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

            var foundAttr = caughtAttributes.FirstOrDefault(a => a.Name == "foo-fact");
            
            Assert.NotNull(foundAttr);
            Assert.Equal("foo-fact-val", foundAttr.Value.ToString());
        }
        
        [Fact]
        public void ShouldAddMarkerWhenDetectInnerExceptions()
        {
            //Arrange
            var eBase = new Exception("foo");
            var e = new Exception("bar", eBase);

            List<string> leMarkers = null;

            var l = Tools.GetLogger((level, id, logEntity, exception, formatter) =>
            {
                leMarkers = logEntity.Markers;
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
            Assert.Contains(leMarkers,s => s == Markers.HasInnerException);
        }
        
        [Fact]
        public void ShouldAddMarkerWhenDetectExceptionsWithoutAssignedId()
        {
            //Arrange
            var e = new Exception("bar");

            List<string> leMarkers = null;

            var l = Tools.GetLogger((level, id, logEntity, exception, formatter) =>
            {
                leMarkers = logEntity.Markers;
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
            Assert.Contains(leMarkers,s => s == Markers.ExceptionHasNoIdentifier);
        }
    }
}
