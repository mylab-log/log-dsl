using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyLab.Db;
using MyLab.Log.Dsl;

namespace TestApi2
{
	public class DataController : ControllerBase
	{
		private IDslLogger<DataController> _logger;

		/// <summary>
		/// Constructor
		/// </summary>
		public DataController(IDslLogger<DataController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		[Route("data")]
		public async Task<IActionResult> GetData()
		{
			return Ok("ok");
		}

	}
}