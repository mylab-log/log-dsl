using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyLab.Db;
using MyLab.Log.Dsl;

namespace TestApi2
{
	public class DataController : ControllerBase
	{
		private IDslLogger<DataController> _logger;
		private IDbManager _db;

		/// <summary>
		/// Constructor
		/// </summary>
		public DataController(IDslLogger<DataController> logger, IDbManager db)
		{
			_logger = logger;
			_db = db;
		}

		[HttpGet]
		[Route("data")]
		public async Task<IActionResult> GetData()
		{
			return Ok("ok");
		}

	}
}