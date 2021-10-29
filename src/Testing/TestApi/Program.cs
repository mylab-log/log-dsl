using LinqToDB.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace TestApi
{
	public class Program
	{
		public static void Main()
		{
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (s, s1, t) => Debug.WriteLine(s, s1);

			var host = CreateHostBuilder(null).Build();
			host.Start();
			Thread.Sleep(2000);
			return;
		}

		public static IHostBuilder CreateHostBuilder(IConfigurationBuilder configBuilder) =>
			Host.CreateDefaultBuilder()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder
				  .UseStartup<Startup>();
				});
	}
}
