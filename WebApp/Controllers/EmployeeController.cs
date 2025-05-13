using Microsoft.AspNetCore.Mvc;
using WebApp.Controllers.Filters;

namespace WebApp.Controllers
{
	[RoleAuthorize("Employee")]
	public class EmployeeController : BaseController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}