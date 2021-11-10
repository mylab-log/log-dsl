using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyLab.Log.Dsl;

namespace TestApi
{
	public partial class DraftController : ControllerBase
	{
		private readonly ITestService _testService;
		private readonly IDslLogger<DraftController> _logger;

		/// <summary>
		/// Constructor
		/// </summary>
		public DraftController(ITestService testService, IDslLogger<DraftController> logger)
		{
			_testService = testService;
			_logger = logger;
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