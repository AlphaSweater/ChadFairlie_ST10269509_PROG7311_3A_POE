using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers.Filters;

namespace WebApp.Controllers
{
	[RoleAuthorize("Farmer")]
	public class FarmerController : BaseController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}