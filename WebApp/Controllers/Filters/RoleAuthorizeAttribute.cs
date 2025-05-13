using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Controllers.Filters
{
	public class RoleAuthorizeAttribute : ActionFilterAttribute
	{
		private readonly string[] _allowedRoles;

		public RoleAuthorizeAttribute(params string[] allowedRoles)
		{
			_allowedRoles = allowedRoles;
		}

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var userRole = context.HttpContext.Session.GetString("UserRole");

			// Check if the user's role is in the allowed roles
			if (string.IsNullOrEmpty(userRole) || !_allowedRoles.Contains(userRole))
			{
				context.Result = new RedirectToActionResult("Unauthorized", "Auth", null);
			}
		}
	}
}