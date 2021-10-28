using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyLab.Log.Dsl;
using Xunit;

namespace Tests
{
    public class LogContextsInjectionBehavior
    {
        [Fact]
        public void ShouldApplyContextForScope()
        {
            //Arrange
            var testCtxVal = Guid.NewGuid().ToString("N");
            var testCtxSrv = new TestCtxService(testCtxVal);

            var lInstance = new TestLogger();

            var sp = new ServiceCollection()
                .AddLogging(lBuilder => lBuilder
                    .AddDsl()
                    .AddProvider(new TestLoggerProvider(lInstance)))
                .AddDslLogContext<TestCtxApplier>()
                .AddScoped(s => testCtxSrv)
                .AddScoped<TestTargetService>()
                .BuildServiceProvider();
            var scope = sp.CreateScope();
            var targetService = scope.ServiceProvider.GetService<TestTargetService>();

            //Act
            targetService.WriteError("foo");

            //Assert
            Assert.Contains(lInstance.LastMessage.Labels, kv => kv.Key == testCtxVal && kv.Value == "true");
        }

        [Fact]
        public void ShouldApplyContextWithoutScope()
        {
            //Arrange
            var testCtxVal = Guid.NewGuid().ToString("N");
            var testCtxSrv = new TestCtxService(testCtxVal);

            var lInstance = new TestLogger();

            var sp = new ServiceCollection()
                .AddLogging(lBuilder => lBuilder
                    .AddDsl()
                    .AddProvider(new TestLoggerProvider(lInstance)))
                .AddDslLogContext<TestCtxApplier>()
                .AddSingleton(testCtxSrv)
                .AddSingleton<TestTargetService>()
                .BuildServiceProvider();
            var targetService = sp.GetService<TestTargetService>();

            //Act
            targetService.WriteError("foo");

            //Assert
            Assert.Contains(lInstance.LastMessage.Labels, kv => kv.Key == testCtxVal && kv.Value == "true");
        }

        class TestCtxApplier : IDslLogContextApplier
        {
            private readonly TestCtxService _ctx;

            public TestCtxApplier(TestCtxService ctx)
            {
                _ctx = ctx;
            }

            public DslExpression Apply(DslExpression dslExpression)
            {
                return dslExpression.AndLabel(_ctx.Value);
            }
        }

        class TestCtxService
        {
            public string Value { get; }

            public TestCtxService(string value)
            {
                Value = value;
            }
        }

        class TestTargetService
        {
            private readonly IDslLogger<TestTargetService> _logger;

            public TestTargetService(IDslLogger<TestTargetService> logger)
            {
                _logger = logger;
            }

            public void WriteError(string msg)
            {
                _logger.Error(msg).Write();
            }
        }
    }
}
