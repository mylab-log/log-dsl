using System;
using Microsoft.Extensions.DependencyInjection;

namespace MyLab.Log.Dsl
{
    /// <summary>
    /// Contains extension methods for integration
    /// </summary>
    public static class IntegrationExtensions
    {
        public static IServiceCollection AddDslLog(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            return services.AddScoped<IDslLogger, DslLogger>()
                .AddScoped(typeof(IDslLogger<>), typeof(DslLogger<>));
        }
    }
}
