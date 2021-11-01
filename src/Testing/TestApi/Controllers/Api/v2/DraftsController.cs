using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infonot.Security;

namespace TestApi
{
	public partial class DraftController : ControllerBase
	{
		private readonly ITestService _testService;

		/// <summary>
		/// Constructor
		/// </summary>
		public DraftController(ITestService testService)
		{
			_testService = testService;
		}

		[HttpGet("data")]
		public async Task<IActionResult> GetData()
		{			
			return Ok(_testService.GetData());
		}

	}

	public interface ITestService
	{
		string GetData();
	}

	class TestService : ITestService
	{
		public string GetData()
		{
			return "some text";
		}
	}
}