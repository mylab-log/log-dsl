using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        private readonly ITestOutputHelper _output;

        /// <summary>
        /// Initializes a new instance of <see cref="DslLoggerInjectionBehavior"/>
        /// </summary>
        public DslLoggerInjectionBehavior(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ShouldIntegrateDslLogger()
        {
            //Arrange
            var lInstance = new TestLogger();

            var sc = new ServiceCollection()
                .AddLogging(l => l
                    .AddDsl()
                    .AddXUnit(_output)
                    .AddProvider(new TestLoggerProvider(lInstance)))
                .BuildServiceProvider();

            //Act
            IDslLogger l = sc.GetService<IDslLogger>();

            l.Error("foo").Write();

            //Assert
            Assert.Equal("foo", lInstance.LastMessage.Message);
        }

        [Fact]
        public void ShouldIntegrateDslLoggerWithCategory()
        {
            //Arrange
            var lInstance = new TestLogger();

            var sc = new ServiceCollection()
                .AddLogging(l => l
                    .AddDsl()
                    .AddXUnit(_output)
                    .AddProvider(new TestLoggerProvider(lInstance)))
                .BuildServiceProvider();

            //Act
            IDslLogger l = sc.GetRequiredService<IDslLogger<DslLoggerInjectionBehavior>>();

            l.Error("foo").Write();

            //Assert
            Assert.Equal("foo", lInstance.LastMessage.Message);
        }
    }
}
