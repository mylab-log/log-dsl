using System;
using MyLab.Log.Dsl;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds context applier for DSL logging
        /// </summary>
        public static IServiceCollection AddDslLogContext<TLogCtx>(this IServiceCollection srv)
            where TLogCtx : class, IDslLogContextApplier
        {
            if (srv == null) throw new ArgumentNullException(nameof(srv));
            return srv.AddSingleton<IDslLogContextApplier, TLogCtx>();
        }
    }
}