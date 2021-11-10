using MyLab.Log.Dsl;

namespace TestApi
{
    class TestLogContextApplier : IDslLogContextApplier
    {
        public DslExpression Apply(DslExpression dslExpression)
        {
            return dslExpression;
        }
    }
}