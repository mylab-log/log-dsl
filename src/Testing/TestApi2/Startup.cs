using LinqToDB.DataProvider.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyLab.Db;
using MyLab.HttpMetrics;
using MyLab.RedisManager;
using MyLab.StatusProvider;
using MyLab.Log.Syslog;
using MyLab.WebErrors;
using Prometheus;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using System;
using LinqToDB;

namespace TestApi2
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers(options =>
			{
				options.AddExceptionProcessing();
			})
			.AddNewtonsoftJson();

			services.AddSingleton(Configuration);

			services.AddHttpContextAccessor();
			services.AddDbTools(Configuration, new MySqlDataProvider(ProviderName.MySql));
			services.AddLogging(loggingBuilder => loggingBuilder
				.AddConsole()
				.AddDebug()
				.AddDsl()
			);

			services.AddUrlBasedHttpMetrics();

            services.Configure<ExceptionProcessingOptions>(o => o.HideError = false);
        }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpMetrics();           // <--- Добавляем сбор общих метрик, связанных с выполнением HTTP запросов
			app.UseUrlBasedHttpMetrics();   // <--- Добавляем сбор метрик в привязке к URL
			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapMetrics();     // <--- Добавляем предоставление метрик
			});
		}
	}
}
