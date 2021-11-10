using MyLab.Log.Dsl;

namespace TestApi
{
    class TestLogContext : IDslLogContext
    {
        public DslExpression Set(DslExpression dslExpression)
        {
            return dslExpression;
        }
    }
}