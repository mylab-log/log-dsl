using System;
using Microsoft.Extensions.Logging;
using MyLab.Log.Dsl;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for <see cref="ILoggingBuilder"/>
    /// </summary>
    public static class LoggingBuilderExtensions
    {
        /// <summary>
        /// Adds DSL logger into services
        /// </summary>
        public static ILoggingBuilder AddDsl(this ILoggingBuilder loggingBuilder)
        {
            if (loggingBuilder == null) throw new ArgumentNullException(nameof(loggingBuilder));

            loggingBuilder.Services  
                .AddSingleton<IDslLogger, DslLogger>()
                .AddSingleton(typeof(IDslLogger<>), typeof(DslLogger<>));

            return loggingBuilder;
        }

        /// <summary>
        /// Adds context applier for DSL logging
        /// </summary>
        public static ILoggingBuilder AddDslCtx<TLogCtx>(this ILoggingBuilder loggingBuilder)
            where TLogCtx : class, IDslLogContext
        {
            if (loggingBuilder == null) throw new ArgumentNullException(nameof(loggingBuilder));
            
            loggingBuilder.Services.AddSingleton<IDslLogContext, TLogCtx>();

            return loggingBuilder;
        }

        /// <summary>
        /// Adds context applier for DSL logging
        /// </summary>
        public static ILoggingBuilder AddDslCtx(this ILoggingBuilder loggingBuilder, IDslLogContext context)
        {
            if (loggingBuilder == null) throw new ArgumentNullException(nameof(loggingBuilder));

            loggingBuilder.Services.AddSingleton<IDslLogContext>(context);

            return loggingBuilder;
        }
    }
}
