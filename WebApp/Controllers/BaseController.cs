using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApp.Services;

namespace WebApp.Controllers
{
	public class BaseController : Controller
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var userRole = HttpContext.Session.GetString("UserRole");
			var userId = HttpContext.Session.GetInt32("UserId");

			if (string.IsNullOrEmpty(userRole) || userId == null)
			{
				context.Result = RedirectToAction("Auth", "Auth");
			}

			base.OnActionExecuting(context);
		}
	}
}