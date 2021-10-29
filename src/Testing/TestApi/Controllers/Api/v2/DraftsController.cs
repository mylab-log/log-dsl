using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infonot.Security;

namespace TestApi
{
	[Authorize]
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

		[HttpPost]
		[Route("v2/drafts")]
		public async Task<IActionResult> CreateDraft(InfonotClientContext context)
		{			
			return Ok();
		}

	}

	public interface ITestService
	{
		void Do();
	}

	class TestService : ITestService
	{
		public void Do()
		{
		}
	}
}