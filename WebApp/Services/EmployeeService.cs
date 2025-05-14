using Microsoft.Extensions.Logging;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services
{
	/// <summary>
	/// Service for managing employee-related operations
	/// </summary>
	public class EmployeeService : IBaseService<Employee, int>
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly ILogger<EmployeeService> _logger;

		public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger)
		{
			_employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <inheritdoc/>
		public async Task<List<Employee>> GetAllAsync()
		{
			try
			{
				return await _employeeRepository.GetAllAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving all employees");
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task<Employee?> GetByIdAsync(int id)
		{
			try
			{
				if (id <= 0)
					throw new ArgumentException("Employee ID must be positive", nameof(id));

				return await _employeeRepository.GetByIdAsync(id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving employee {EmployeeId}", id);
				throw;
			}
		}

		/// <summary>
		/// Gets an employee by their email address
		/// </summary>
		public async Task<Employee?> GetByEmailAsync(string email)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(email))
					throw new ArgumentException("Email is required", nameof(email));

				return await _employeeRepository.GetByEmailAsync(email);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving employee by email {Email}", email);
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task<Employee> AddAsync(Employee employee)
		{
			try
			{
				if (employee == null)
					throw new ArgumentNullException(nameof(employee));

				ValidateEmployee(employee);

				employee.CreatedOn = DateTime.UtcNow;
				await _employeeRepository.AddAsync(employee);
				await _employeeRepository.SaveChangesAsync();

				_logger.LogInformation("Employee {EmployeeId} created successfully", employee.EmployeeId);
				return employee;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating employee");
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task<Employee> UpdateAsync(Employee employee)
		{
			try
			{
				if (employee == null)
					throw new ArgumentNullException(nameof(employee));

				ValidateEmployee(employee);

				await _employeeRepository.UpdateAsync(employee);
				await _employeeRepository.SaveChangesAsync();

				_logger.LogInformation("Employee {EmployeeId} updated successfully", employee.EmployeeId);
				return employee;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating employee {EmployeeId}", employee?.EmployeeId);
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task DeleteAsync(int id)
		{
			try
			{
				if (id <= 0)
					throw new ArgumentException("Employee ID must be positive", nameof(id));

				await _employeeRepository.DeleteAsync(id);
				await _employeeRepository.SaveChangesAsync();

				_logger.LogInformation("Employee {EmployeeId} deleted successfully", id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting employee {EmployeeId}", id);
				throw;
			}
		}

		/// <summary>
		/// Filters employees based on a predicate
		/// </summary>
		public async Task<List<Employee>> FilterEmployeesAsync(Func<Employee, bool> predicate)
		{
			try
			{
				if (predicate == null)
					throw new ArgumentNullException(nameof(predicate));

				return await _employeeRepository.FilterAsync(e => predicate(e));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error filtering employees");
				throw;
			}
		}

		private static void ValidateEmployee(Employee employee)
		{
			if (string.IsNullOrWhiteSpace(employee.FirstName))
				throw new ArgumentException("First name is required", nameof(employee));

			if (string.IsNullOrWhiteSpace(employee.LastName))
				throw new ArgumentException("Last name is required", nameof(employee));

			if (string.IsNullOrWhiteSpace(employee.Email))
				throw new ArgumentException("Email is required", nameof(employee));

			if (string.IsNullOrWhiteSpace(employee.PasswordHash))
				throw new ArgumentException("Password hash is required", nameof(employee));
		}
	}
}