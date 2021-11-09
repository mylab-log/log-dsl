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
		public static void Main(string[] args)
		{
			DataConnection.TurnTraceSwitchOn();
			DataConnection.WriteTraceLine = (s, s1, t) => Debug.WriteLine(s, s1);

			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder
				  .UseStartup<Startup>();
				});
	}
}
