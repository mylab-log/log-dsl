using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTests
{
	public class LogContextsInWebHostBehavior : IClassFixture<WebApplicationFactory<TestApi.Startup>>
	{
		private readonly WebApplicationFactory<TestApi.Startup> _factory;

		public LogContextsInWebHostBehavior(WebApplicationFactory<TestApi.Startup> factory)
		{
			_factory = factory;
		}

		[Fact]
		public async Task ShouldBuildWebHost()
		{
			// Arrange
			var client = _factory.CreateClient();

			// Act
			var response  = await client.GetAsync("data");
			
			var text = await response.Content.ReadAsStringAsync();

			// Assert
			Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
			Assert.Equal("some text", text);
		}

	}

}
