using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services
{
	public class EmployeeService
	{
		private readonly IEmployeeRepository _employeeRepository;

		public EmployeeService(IEmployeeRepository employeeRepository)
		{
			_employeeRepository = employeeRepository;
		}

		public async Task<List<Employee>> GetAllEmployeesAsync()
		{
			return await _employeeRepository.GetAllAsync();
		}

		public async Task<Employee?> GetEmployeeByIdAsync(int id)
		{
			return await _employeeRepository.GetByIdAsync(id);
		}

		public async Task AddEmployeeAsync(Employee employee)
		{
			await _employeeRepository.AddAsync(employee);
			await _employeeRepository.SaveChangesAsync();
		}

		public async Task UpdateEmployeeAsync(Employee employee)
		{
			await _employeeRepository.UpdateAsync(employee);
			await _employeeRepository.SaveChangesAsync();
		}

		public async Task DeleteEmployeeAsync(int id)
		{
			await _employeeRepository.DeleteAsync(id);
			await _employeeRepository.SaveChangesAsync();
		}

		public async Task<List<Employee>> FilterEmployeesAsync(Func<Employee, bool> predicate)
		{
			return await _employeeRepository.FilterAsync(predicate);
		}
	}
}