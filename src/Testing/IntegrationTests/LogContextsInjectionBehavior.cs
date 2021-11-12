using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyLab.Log.Dsl;
using TestApi;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
	public class LogContextsInWebHostBehavior : IClassFixture<WebApplicationFactory<TestApi.Startup>>
	{
		private readonly WebApplicationFactory<TestApi.Startup> _factory;
        private readonly ITestOutputHelper _output;

        public LogContextsInWebHostBehavior(WebApplicationFactory<TestApi.Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

		[Fact]
		public async Task ShouldBuildWebHost()
		{
			// Arrange
			var client = _factory.WithWebHostBuilder(
                b => b.ConfigureServices(s => 
                    s.AddLogging(l => l
                        .AddXUnit(_output)
                        .AddFilter(f => true)
                        ))
                ).CreateClient();

			// Act
			var response  = await client.GetAsync("data");
			
			var text = await response.Content.ReadAsStringAsync();

			// Assert
			Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
			Assert.Equal("some text", text);
		}


		[Fact]
		public async Task ShouldAddFactByDslContext()
		{
			// Arrange
			var client = _factory.WithWebHostBuilder(
				b => b.ConfigureServices(s =>
					s.AddLogging(l => l
						.AddXUnit(_output)
						.AddFilter(f => true)
					))
			).CreateClient();

			// Act
			var response = await client.GetAsync("test-fact");

			var text = await response.Content.ReadAsStringAsync();
			_output.WriteLine(text);

			// Assert
			Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(TestLogContextApplier.TestFactValue, text);
		}
	}
}
