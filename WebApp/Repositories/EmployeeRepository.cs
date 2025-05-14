using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Repositories
{
	public class EmployeeRepository : IEmployeeRepository
	{
		private readonly AgriDbContext _context;

		public EmployeeRepository(AgriDbContext context)
		{
			_context = context;
		}

		public async Task<List<Employee>> GetAllAsync()
		{
			return await _context.Employees.Where(e => !e.IsDeleted).ToListAsync();
		}

		public async Task<Employee?> GetByIdAsync(int id)
		{
			return await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id && !e.IsDeleted);
		}

		public async Task<Employee?> GetByEmailAsync(string email)
		{
			return await _context.Employees.FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower() && !e.IsDeleted);
		}

		public async Task AddAsync(Employee employee)
		{
			await _context.Employees.AddAsync(employee);
		}

		public async Task UpdateAsync(Employee employee)
		{
			_context.Employees.Update(employee);
		}

		public async Task DeleteAsync(int id)
		{
			var employee = await GetByIdAsync(id);
			if (employee != null)
			{
				employee.IsDeleted = true;
				_context.Employees.Update(employee);
			}
		}

		public async Task<List<Employee>> FilterAsync(Func<Employee, bool> predicate)
		{
			return _context.Employees.Where(predicate).Where(e => !e.IsDeleted).ToList();
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}