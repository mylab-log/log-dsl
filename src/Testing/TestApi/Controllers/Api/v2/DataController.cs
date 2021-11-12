using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyLab.Log.Dsl;

namespace TestApi
{
	public partial class DataController : ControllerBase
	{
		private readonly ITestService _testService;
		private readonly IDslLogger<DataController> _logger;

		/// <summary>
		/// Constructor
		/// </summary>
		public DataController(ITestService testService, IDslLogger<DataController> logger)
		{
			_testService = testService;
			_logger = logger;
		}

		[HttpGet("data")]
		public async Task<IActionResult> GetData()
		{
			return Ok(_testService.GetData());
		}

		[HttpGet("test-fact")]
		public async Task<IActionResult> GetTestFAct()
		{
			return Ok(_testService.GetTestFact());
		}
		
	}

	public interface ITestService
	{
		string GetData();
		string GetTestFact();
	}

	class TestService : ITestService
	{
		private IDslLogger<TestService> _logger;

		public TestService(IDslLogger<TestService> logger)
		{
			_logger= logger;
		}
		public string GetData()
		{
			return "some text";
		}

		public string GetTestFact()
		{
			var log = _logger.Error("ошибка").Create();
			return log.Facts.ContainsKey("test-fact") ?  log.Facts["test-fact"].ToString() : "<no fact found>";
		}
	}
}