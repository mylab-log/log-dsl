using System;
using System.Linq;
using MyLab.Log;
using MyLab.Log.Dsl;
using Xunit;

namespace Tests
{
    public partial class DslLoggerBehavior
    {
        [Fact]
        public void ShouldLogAction()
        {
            //Arrange
            var logger = CreateCoreLogger();

            //Act
            var expression = logger.Action("Foo");

            expression.Write();
            var log = expression.Create();

            //Assert
            Assert.Equal("Foo", log.Message);
        }

        [Theory]
        [InlineData("debug", PredefinedLogLevels.Debug)]
        [InlineData("warning", PredefinedLogLevels.Warning)]
        [InlineData("error", PredefinedLogLevels.Error)]
        [InlineData("act", null)]
        public void ShouldSetLevelLabel(string levelKey, string levelValue)
        {
            //Arrange
            var logger = CreateCoreLogger();

            //Act

            DslExpression expr;

            switch (levelKey)
            {
                case "debug": 
                    expr = logger.Debug("Trigger was triggered");
                    break;
                case "act":
                    expr = logger.Action("I did it!");
                    break;
                case "warning":
                    expr = logger.Warning("Invalid client request");
                    break;
                case "error":
                    expr = logger.Error("Backend server connection error");
                    break;
                default: 
                    throw new IndexOutOfRangeException();
            }

            expr.Write();
            var log = expr.Create();

            log.Labels.TryGetValue(PredefinedLabels.LogLevel, out var foundLogLevel);

            //Assert
            Assert.Equal(levelValue, foundLogLevel);
        }

        [Fact]
        public void ShouldAddReasonException()
        {
            //Arrange
            var logger = CreateCoreLogger();

            Exception caughtException;
            try
            {
                throw new Exception("Bar");
            }
            catch (Exception e)
            {
                caughtException = e;
            }

            //Act
            var expression = logger
                .Action("Foo")
                .BecauseOf(caughtException);

            expression.Write();
            var log = expression.Create();

            //Assert
            Assert.NotNull(log.Exception);
            Assert.Equal("Bar", log.Exception.Message);
        }

        [Fact]
        public void ShouldAddNamedFacts()
        {
            //Arrange
            var logger = CreateCoreLogger();

            //Act
            var expression = logger
                .Action("Foo")
                .AndFactIs("bar", "baz");

            expression.Write();
            var log = expression.Create();

            //Assert
            Assert.Equal("baz", log.Facts["bar"]);
        }

        [Fact]
        public void ShouldAddStringCondition()
        {
            //Arrange
            var logger = CreateCoreLogger();

            //Act
            var expression = logger
                .Action("Foo")
                .AndFactIs("The day is rainy");

            expression.Write();
            var log = expression.Create();
            var conditions = (string[]) log.Facts[PredefinedFacts.Conditions];

            //Assert
            Assert.Contains(conditions, s => s == "The day is rainy");
        }

        [Fact]
        public void ShouldAddExpressionCondition()
        {
            //Arrange
            var logger = CreateCoreLogger();

            //Act
            var expression = logger
                .Action("Foo")
                .AndFactIs(() => DateTime.Now > DateTime.MinValue);

            expression.Write();
            var log = expression.Create();
            var conditions = (string[])log.Facts[PredefinedFacts.Conditions];

            //Assert
            Assert.Contains(conditions, s => s == "'DateTime.Now > DateTime.MinValue' is True");
        }

        [Fact]
        public void ShouldAddLabel()
        {
            //Arrange
            var logger = CreateCoreLogger();

            //Act
            var expression = logger
                .Action("Foo")
                .AndLabel("bar", "baz");

            expression.Write();
            var log = expression.Create();

            //Assert
            Assert.Equal("baz", log.Labels["bar"]);
        }

        [Fact]
        public void ShouldAddFlagLabel()
        {
            //Arrange
            var logger = CreateCoreLogger();

            //Act
            var expression = logger
                .Action("Foo")
                .AndLabel("bar");

            expression.Write();
            var log = expression.Create();

            //Assert
            Assert.Equal("true", log.Labels["bar"]);
        }

        [Fact]
        public void FactDemo()
        {
            //Arrange
            var logger = CreateCoreLogger();
            int debugParameter = 1;

            //Act

            logger
                .Action("Something done")
                .AndFactIs("foo", "bar")
                .AndFactIs("The day is rainy")
                .AndFactIs(() => debugParameter > 5)
                .Write();
        }

        [Fact]
        public void LabelDemo()
        {
            //Arrange
            var logger = CreateCoreLogger();

            //Act

            logger
                .Action("Something done")
                .AndLabel("priority", "high")
                .AndLabel("good_message")
                .Write();
        }
    }
}
