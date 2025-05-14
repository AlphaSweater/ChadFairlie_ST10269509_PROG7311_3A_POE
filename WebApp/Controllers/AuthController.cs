using Microsoft.AspNetCore.Mvc;
using WebApp.Services;

namespace WebApp.Controllers
{
	public class AuthController : Controller
	{
		private readonly AuthService _userSessionService;

		public AuthController(AuthService userSessionService)
		{
			_userSessionService = userSessionService;
		}

		public IActionResult Auth()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(string email, string password)
		{
			// Test logic for login
			if (email == "admin@gmail.com" && password == "password")
			{
				_userSessionService.SetUserIdRole(1, "Employee"); // Set user ID and role in session

				return RedirectToAction("Index", "Employee");
			}
			else if (email == "farmer@gmail.com" && password == "password")
			{
				_userSessionService.SetUserIdRole(2, "Farmer"); // Set user ID and role in session

				return RedirectToAction("Index", "Farmer");
			}
			else
			{
				ViewBag.LoginError = "Invalid email or password.";
				return View("Auth");
			}
		}

		public IActionResult SkipFarmerLogin()
		{
			_userSessionService.SetUserIdRole(2, "Farmer"); // Set user ID and role in session
			return RedirectToAction("Index", "Farmer");
		}

		public IActionResult SkipAdminLogin()
		{
			_userSessionService.SetUserIdRole(1, "Employee"); // Set user ID and role in session
			return RedirectToAction("Index", "Employee");
		}

		public IActionResult Logout()
		{
			_userSessionService.ClearSession();
			return RedirectToAction("Auth");
		}

		[HttpPost]
		public IActionResult SignUp(string name, string surname, string email, string password, string confirmPassword)
		{
			// Test logic for sign-up
			if (password != confirmPassword)
			{
				ViewBag.SignUpError = "Passwords do not match.";
				return View("Auth");
			}

			// Simulate successful sign-up
			ViewBag.SignUpSuccess = "Account created successfully. Please log in.";
			return View("Auth");
		}

		public IActionResult Unauthorized()
		{
			return View();
		}
	}
}