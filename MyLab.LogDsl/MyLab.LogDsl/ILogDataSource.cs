namespace MyLab.LogDsl
{
    /// <summary>
    /// Determines a log data source
    /// </summary>
    public interface ILogDataSource
    {
        /// <summary>
        /// Adds log data to <see cref="DslLogEntityBuilder"/>
        /// </summary>
        void AddLogData(DslLogEntityBuilder builder);
    }
}