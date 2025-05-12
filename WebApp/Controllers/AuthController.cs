using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
	public class AuthController : Controller
	{
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
				// Redirect to a test page or home page
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ViewBag.LoginError = "Invalid email or password.";
				return View("Auth");
			}
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
	}
}