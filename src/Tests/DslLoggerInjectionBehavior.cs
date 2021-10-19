using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyLab.Log;
using MyLab.Log.Dsl;
using Xunit;
using Xunit.Abstractions;

namespace Tests
{
    public partial class DslLoggerInjectionBehavior
    {
        [Fact]
        public void ShouldIntegrateDslLogger()
        {
            //Arrange
            var sc = new ServiceCollection()
                .AddLogging(l => l
                    .AddDsl()
                    .AddXUnit(_output)
                    .AddProvider(new TestLoggerProvider()))
                .BuildServiceProvider();

            //Act
            IDslLogger l = sc.GetService<IDslLogger>();

            l.Error("foo").Write();

            //Assert
            Assert.Equal("foo", TestLogger.Instance.LastMessage.Message);
        }

        [Fact]
        public void ShouldIntegrateDslLoggerWithCategory()
        {
            //Arrange
            var sc = new ServiceCollection()
                .AddLogging(l => l
                    .AddDsl()
                    .AddXUnit(_output)
                    .AddProvider(new TestLoggerProvider()))
                .BuildServiceProvider();

            //Act
            IDslLogger l = sc.GetRequiredService<IDslLogger<DslLoggerInjectionBehavior>>();

            l.Error("foo").Write();

            //Assert
            Assert.Equal("foo", TestLogger.Instance.LastMessage.Message);
        }
    }
}
