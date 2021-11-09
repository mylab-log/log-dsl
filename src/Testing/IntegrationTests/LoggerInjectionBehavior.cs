using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTests
{
	public class LoggerInjectionBehavior : IClassFixture<WebApplicationFactory<TestApi2.Startup>>
	{
		private readonly WebApplicationFactory<TestApi2.Startup> _factory;

		public LoggerInjectionBehavior(WebApplicationFactory<TestApi2.Startup> factory)
		{
			_factory = factory;
		}

		[Fact]
		public async Task ShouldNotReturn500()
		{
			// Arrange
			var client = _factory.CreateClient();

			// Act
			var response  = await client.GetAsync("data");
			
			var text = await response.Content.ReadAsStringAsync();

			// Assert
			Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
			Assert.Equal("ok", text);
		}

	}

}
