using Microsoft.AspNetCore.Identity;
using WebApp.Models;

namespace WebApp.Services
{
	public class AuthService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly PasswordHasher<object> _passwordHasher;
		private readonly FarmerService _farmerService;
		private readonly EmployeeService _employeeService;

		public AuthService(IHttpContextAccessor httpContextAccessor, FarmerService farmerService, EmployeeService employeeService)
		{
			_httpContextAccessor = httpContextAccessor;
			_passwordHasher = new PasswordHasher<object>();
			_farmerService = farmerService;
			_employeeService = employeeService;
		}

		public async Task<bool> LoginUserAsync(string email, string password)
		{
			// Check if the user is a farmer
			var farmer = await _farmerService.GetByEmailAsync(email);

			if (farmer != null && VerifyPassword(farmer.PasswordHash, password))
			{
				// Set session data for farmer
				_httpContextAccessor.HttpContext?.Session.SetInt32("UserId", farmer.FarmerId);
				_httpContextAccessor.HttpContext?.Session.SetString("UserRole", "Farmer");
				return true; // Login successful as farmer
			}

			// Check if the user is an employee
			var employee = await _employeeService.GetByEmailAsync(email);

			if (employee != null && VerifyPassword(employee.PasswordHash, password))
			{
				// Set session data for employee
				_httpContextAccessor.HttpContext?.Session.SetInt32("UserId", employee.EmployeeId);
				_httpContextAccessor.HttpContext?.Session.SetString("UserRole", "Employee");
				return true; // Login successful as employee
			}

			return false; // Login failed
		}

		public async Task<bool> RegisterEmployeeAsync(Employee employee)
		{
			// Hash the password before saving
			employee.PasswordHash = HashPassword(employee.PasswordHash);
			employee.CreatedOn = DateTime.UtcNow; // Set CreatedOn to current UTC time
			await _employeeService.AddAsync(employee);
			return true; // Registration successful
		}

		public async Task<bool> IsEmailInUseAsync(string email)
		{
			// Check if the email exists in farmers
			var farmerExists = (await _farmerService.FilterFarmersAsync(null, null))
				.Any(f => f.Email == email && !f.IsDeleted);

			if (farmerExists)
			{
				return true; // Email is in use by a farmer
			}

			// Check if the email exists in employees
			var employeeExists = (await _employeeService.FilterEmployeesAsync(e => e.Email == email && !e.IsDeleted))
				.Any();

			return employeeExists; // Email is in use by an employee
		}

		public string HashPassword(string password)
		{
			// Use a dummy object as the user instance for hashing
			var dummyUser = new object();
			return _passwordHasher.HashPassword(dummyUser, password);
		}

		private bool VerifyPassword(string hashedPassword, string inputPassword)
		{
			var dummyUser = new object();
			var result = _passwordHasher.VerifyHashedPassword(dummyUser, hashedPassword, inputPassword);
			return result == PasswordVerificationResult.Success;
		}

		public void SetUserIdRole(int Id, string Role)
		{
			_httpContextAccessor.HttpContext?.Session.SetInt32("UserId", Id);
			_httpContextAccessor.HttpContext?.Session.SetString("UserRole", Role);
		}

		public (int, string?) GetUserIdRole()
		{
			int? userId = _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");
			string? userRole = _httpContextAccessor.HttpContext?.Session.GetString("UserRole");
			return (userId ?? 0, userRole);
		}

		public void ClearSession()
		{
			_httpContextAccessor.HttpContext?.Session.Clear();
		}
	}
}