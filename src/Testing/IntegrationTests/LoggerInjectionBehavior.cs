using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
	public class LoggerInjectionBehavior : IClassFixture<WebApplicationFactory<TestApi2.Startup>>
	{
		private readonly WebApplicationFactory<TestApi2.Startup> _factory;
        private readonly ITestOutputHelper _output;

        public LoggerInjectionBehavior(WebApplicationFactory<TestApi2.Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
        }

		[Fact]
		public async Task ShouldNotReturn500()
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
			_output.WriteLine(text);

			// Assert
			Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
			Assert.Equal("ok", text);
		}

	}

}
