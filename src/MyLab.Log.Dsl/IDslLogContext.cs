namespace MyLab.Log.Dsl
{
    /// <summary>
    /// Applies context parameters to <see cref="DslExpression"/>
    /// </summary>
    public interface IDslLogContext
    {
        /// <summary>
        /// Applies context parameters to specified <see cref="DslExpression"/>
        /// </summary>
        DslExpression Set(DslExpression dslExpression);
    }
}
