using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
	/// <summary>
	/// Controller responsible for handling authentication-related actions such as login, logout, and sign-up.
	/// </summary>
	public class AuthController : Controller
	{
		private readonly AuthService _authService;

		// TempData keys for managing UI state and error messages
		private const string ShowLoginFormKey = "ShowLoginForm";

		private const string ShowSignUpFormKey = "ShowSignUpForm";
		private const string LoginErrorKey = "LoginError";
		private const string LoginErrorsKey = "LoginErrors";
		private const string LoginSuccessKey = "LoginSuccess";
		private const string SignUpErrorKey = "SignUpError";
		private const string SignUpErrorsKey = "SignUpErrors";

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthController"/> class.
		/// </summary>
		/// <param name="authService">Service for handling authentication logic.</param>
		public AuthController(AuthService authService)
		{
			_authService = authService;
		}

		/// <summary>
		/// Displays the authentication page (login and sign-up forms).
		/// </summary>
		/// <returns>The authentication view.</returns>
		public IActionResult Auth() => View();

		/// <summary>
		/// Handles user login requests.
		/// </summary>
		/// <param name="email">The user's email address.</param>
		/// <param name="password">The user's password.</param>
		/// <returns>Redirects to the appropriate page based on login success or failure.</returns>
		[HttpPost]
		public async Task<IActionResult> Login(string email, string password)
		{
			// Validate input and collect errors
			var errors = ValidateLogin(email, password);

			if (errors.Any())
			{
				// Store errors in TempData and show the login form
				SetTempData(LoginErrorsKey, errors);
				TempData[ShowLoginFormKey] = true;
				return RedirectToAction(nameof(Auth));
			}

			// Attempt to log in the user
			if (await _authService.LoginUserAsync(email, password))
			{
				// Retrieve user ID and role, then redirect based on role
				var (userId, role) = _authService.GetUserIdRole();

				return role switch
				{
					"Farmer" => RedirectToAction("Index", "Farmer"),
					"Employee" => RedirectToAction("Index", "Employee"),
					_ => RedirectToAction(nameof(Auth))
				};
			}

			// Login failed, show error message
			TempData[LoginErrorKey] = "Invalid email or password.";
			TempData[ShowLoginFormKey] = true;
			return RedirectToAction(nameof(Auth));
		}

		/// <summary>
		/// Logs out the current user by clearing the session.
		/// </summary>
		/// <returns>Redirects to the authentication page.</returns>
		public IActionResult Logout()
		{
			_authService.ClearSession();
			return RedirectToAction(nameof(Auth));
		}

		/// <summary>
		/// Handles user sign-up requests.
		/// </summary>
		/// <param name="name">The user's first name.</param>
		/// <param name="surname">The user's last name.</param>
		/// <param name="email">The user's email address.</param>
		/// <param name="password">The user's password.</param>
		/// <param name="confirmPassword">The confirmation of the user's password.</param>
		/// <returns>Redirects to the appropriate page based on sign-up success or failure.</returns>
		[HttpPost]
		public async Task<IActionResult> SignUp(string name, string surname, string email, string password, string confirmPassword)
		{
			// Validate input and collect errors
			var errors = ValidateSignUp(name, surname, email, password, confirmPassword);

			if (errors.Any())
			{
				// Store errors in TempData and show the sign-up form
				SetSignUpErrors(errors);
				return RedirectToAction(nameof(Auth));
			}

			// Check if the email is already in use
			if (await _authService.IsEmailInUseAsync(email))
			{
				errors["email"] = "Email is already in use.";
				SetSignUpErrors(errors);
				return RedirectToAction(nameof(Auth));
			}

			// Create a new employee object
			var newEmployee = new Employee
			{
				FirstName = name,
				LastName = surname,
				Email = email,
				PasswordHash = password,
				CreatedOn = DateTime.UtcNow,
				IsDeleted = false
			};

			// Attempt to register the new employee
			if (await _authService.RegisterEmployeeAsync(newEmployee))
			{
				TempData[ShowLoginFormKey] = true;
				TempData[LoginSuccessKey] = "Account created successfully. Please log in.";
			}
			else
			{
				TempData[ShowSignUpFormKey] = true;
				TempData[SignUpErrorKey] = "An error occurred during sign-up. Please try again.";
			}

			return RedirectToAction(nameof(Auth));
		}

		/// <summary>
		/// Displays the unauthorized access page.
		/// </summary>
		/// <returns>The unauthorized view.</returns>
		public IActionResult Unauthorized() => View();

		// ---------------------- Helpers ----------------------

		/// <summary>
		/// Validates login input fields.
		/// </summary>
		/// <param name="email">The user's email address.</param>
		/// <param name="password">The user's password.</param>
		/// <returns>A dictionary of validation errors, if any.</returns>
		private Dictionary<string, string> ValidateLogin(string email, string password)
		{
			var errors = new Dictionary<string, string>();

			if (string.IsNullOrWhiteSpace(email))
				errors["email"] = "Email is required.";

			if (string.IsNullOrWhiteSpace(password))
				errors["password"] = "Password is required.";

			return errors;
		}

		/// <summary>
		/// Validates sign-up input fields.
		/// </summary>
		/// <param name="name">The user's first name.</param>
		/// <param name="surname">The user's last name.</param>
		/// <param name="email">The user's email address.</param>
		/// <param name="password">The user's password.</param>
		/// <param name="confirmPassword">The confirmation of the user's password.</param>
		/// <returns>A dictionary of validation errors, if any.</returns>
		private Dictionary<string, string> ValidateSignUp(string name, string surname, string email, string password, string confirmPassword)
		{
			var errors = new Dictionary<string, string>();

			if (string.IsNullOrWhiteSpace(name))
				errors["name"] = "Name is required.";

			if (string.IsNullOrWhiteSpace(surname))
				errors["surname"] = "Surname is required.";

			if (string.IsNullOrWhiteSpace(email))
				errors["email"] = "Email is required.";
			else if (!IsValidEmail(email))
				errors["email"] = "Invalid email address.";

			if (string.IsNullOrWhiteSpace(password))
				errors["password"] = "Password is required.";

			if (password != confirmPassword)
				errors["confirmPassword"] = "Passwords do not match.";

			return errors;
		}

		/// <summary>
		/// Validates the format of an email address.
		/// </summary>
		/// <param name="email">The email address to validate.</param>
		/// <returns>True if the email is valid, otherwise false.</returns>
		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Stores sign-up errors in TempData for display on the UI.
		/// </summary>
		/// <param name="errors">The dictionary of validation errors.</param>
		private void SetSignUpErrors(Dictionary<string, string> errors)
		{
			TempData[ShowSignUpFormKey] = true;
			TempData[SignUpErrorsKey] = JsonSerializer.Serialize(errors);
			TempData[SignUpErrorKey] = "Please fix the errors and try again.";
		}

		/// <summary>
		/// Stores a value in TempData, serializing it if necessary.
		/// </summary>
		/// <param name="key">The TempData key.</param>
		/// <param name="value">The value to store.</param>
		private void SetTempData(string key, object value)
		{
			if (value is string str)
				TempData[key] = str;
			else
				TempData[key] = JsonSerializer.Serialize(value);
		}
	}
}