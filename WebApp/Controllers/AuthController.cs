using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
	public class AuthController : Controller
	{
		private readonly AuthService _authService;

		public AuthController(AuthService authService)
		{
			_authService = authService;
		}

		public IActionResult Auth()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(string email, string password)
		{
			var errors = new Dictionary<string, string>();

			if (string.IsNullOrEmpty(email))
			{
				errors["email"] = "Email is required.";
			}
			if (string.IsNullOrEmpty(password))
			{
				errors["password"] = "Password is required.";
			}

			if (errors.Any())
			{
				// Redirect back to the login page with error messages
				TempData["LoginErrors"] = errors;
				TempData["ShowLoginForm"] = true;
				return RedirectToAction("Auth");
			}

			var loginSuccess = await _authService.LoginUserAsync(email, password);
			if (loginSuccess)
			{
				var (userId, role) = _authService.GetUserIdRole();
				// Redirect based on the user's role
				if (role == "Farmer")
				{
					return RedirectToAction("Index", "Farmer");
				}
				else if (role == "Employee")
				{
					return RedirectToAction("Index", "Employee");
				}
			}

			// Redirect back to the login page with a generic error message
			TempData["LoginError"] = "Invalid email or password.";
			TempData["ShowLoginForm"] = true;
			return RedirectToAction("Auth");
		}

		public IActionResult Logout()
		{
			_authService.ClearSession();
			return RedirectToAction("Auth");
		}

		[HttpPost]
		public async Task<IActionResult> SignUp(string name, string surname, string email, string password, string confirmPassword)
		{
			var errors = new Dictionary<string, string>();

			if (string.IsNullOrWhiteSpace(name))
				errors["name"] = "Name is required.";

			if (string.IsNullOrWhiteSpace(surname))
				errors["surname"] = "Surname is required.";

			if (string.IsNullOrWhiteSpace(email))
				errors["email"] = "Email is required.";
			else if (!email.Contains("@"))
				errors["email"] = "Invalid email address.";

			if (string.IsNullOrWhiteSpace(password))
				errors["password"] = "Password is required.";

			if (password != confirmPassword)
				errors["confirmPassword"] = "Passwords do not match.";

			if (errors.Any())
			{
				TempData["ShowSignUpForm"] = true;
				TempData["SignUpErrors"] = System.Text.Json.JsonSerializer.Serialize(errors);
				TempData["SignUpError"] = "Please fix the errors and try again.";
				return RedirectToAction("Auth");
			}

			// Check if the email already exists in the database
			var existingUser = _authService.IsEmailInUseAsync(email).Result;
			if (existingUser)
			{
				errors["email"] = "Email is already in use.";
				TempData["ShowSignUpForm"] = true;
				TempData["SignUpErrors"] = System.Text.Json.JsonSerializer.Serialize(errors);
				TempData["SignUpError"] = "Please fix the errors and try again.";
				return RedirectToAction("Auth");
			}

			var newEmployee = new Employee
			{
				FirstName = name,
				LastName = surname,
				Email = email,
				PasswordHash = password,
				CreatedOn = DateTime.UtcNow,
				IsDeleted = false
			};

			// Register the new employee
			if (await _authService.RegisterEmployeeAsync(newEmployee))
			{
				// successful sign-up
				TempData["ShowLoginForm"] = true;
				TempData["LoginSuccess"] = "Account created successfully. Please log in.";
				return RedirectToAction("Auth");
			}
			else
			{
				// sign-up failed
				TempData["ShowSignUpForm"] = true;
				TempData["SignUpError"] = "An error occurred during sign-up. Please try again.";
				return RedirectToAction("Auth");
			}
		}

		public IActionResult Unauthorized()
		{
			return View();
		}
	}
}