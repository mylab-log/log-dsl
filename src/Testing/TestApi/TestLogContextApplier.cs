using MyLab.Log.Dsl;
using System;

namespace TestApi
{
    public class TestLogContextApplier : IDslLogContextApplier
    {
        public DslExpression Apply(DslExpression dslExpression)
        {
            return dslExpression.AndFactIs("test-fact", TestFactValue);
        }

        public static string TestFactValue = Guid.NewGuid().ToString("N");
    }
}