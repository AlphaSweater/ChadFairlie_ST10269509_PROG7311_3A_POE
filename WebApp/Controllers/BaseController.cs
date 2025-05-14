using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Controllers
{
	/// <summary>
	/// Base controller that provides common functionality for all controllers in the application.
	/// </summary>
	public class BaseController : Controller
	{
		/// <summary>
		/// Executes before an action method is called to ensure the user is authenticated.
		/// </summary>
		/// <param name="context">The context of the action being executed.</param>
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			// Retrieve the user's role and ID from the session
			var userRole = HttpContext.Session.GetString("UserRole");
			var userId = HttpContext.Session.GetInt32("UserId");

			// If the user is not authenticated, redirect to the authentication page
			if (string.IsNullOrEmpty(userRole) || userId == null)
			{
				context.Result = RedirectToAction("Auth", "Auth");
			}

			// Call the base implementation to allow further processing
			base.OnActionExecuting(context);
		}
	}
}