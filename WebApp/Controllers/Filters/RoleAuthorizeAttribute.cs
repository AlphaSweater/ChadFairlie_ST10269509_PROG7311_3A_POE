using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApp.Controllers.Filters
{
	/// <summary>
	/// Custom action filter attribute to restrict access to actions based on user roles.
	/// </summary>
	public class RoleAuthorizeAttribute : ActionFilterAttribute
	{
		private readonly string[] _allowedRoles;

		/// <summary>
		/// Initializes a new instance of the <see cref="RoleAuthorizeAttribute"/> class.
		/// </summary>
		/// <param name="allowedRoles">The roles that are allowed to access the action.</param>
		public RoleAuthorizeAttribute(params string[] allowedRoles)
		{
			_allowedRoles = allowedRoles;
		}

		/// <summary>
		/// Called before the action method is executed to check if the user has the required role.
		/// </summary>
		/// <param name="context">The context of the action being executed.</param>
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			// Retrieve the user's role from the session
			var userRole = context.HttpContext.Session.GetString("UserRole");

			// If the user's role is not in the allowed roles or is missing, redirect to the Unauthorized page
			if (string.IsNullOrEmpty(userRole) || !_allowedRoles.Contains(userRole))
			{
				context.Result = new RedirectToActionResult("Unauthorized", "Auth", null);
			}
		}
	}
}