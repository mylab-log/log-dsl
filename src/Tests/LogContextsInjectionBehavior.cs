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
        public void ShouldApplyContext()
        {
            //Arrange
            var testCtxVal = Guid.NewGuid().ToString("N");
            var testCtxSrv = new TestCtxService(testCtxVal);

            var lInstance = new TestLogger();

            var sp = new ServiceCollection()
                .AddLogging(l => l
                    .AddDsl()
                    .AddProvider(new TestLoggerProvider(lInstance)))
                .AddDslLogContext<TestCtxApplier>()
                .AddScoped(sp => testCtxSrv)
                .BuildServiceProvider();
            var scope = sp.CreateScope();
            IDslLogger l = scope.ServiceProvider.GetService<IDslLogger<LogContextsInjectionBehavior>>();

            //Act
            l.Error("foo").Write();

            //Assert
            Assert.Contains(lInstance.LastMessage.Labels, l => l.Key == testCtxVal && l.Value == "true");
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
    }
}
