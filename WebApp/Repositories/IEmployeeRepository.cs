using WebApp.Models;

namespace WebApp.Repositories
{
	public interface IEmployeeRepository
	{
		Task<List<Employee>> GetAllAsync();

		Task<Employee?> GetByIdAsync(int id);

		Task AddAsync(Employee employee);

		Task UpdateAsync(Employee employee);

		Task DeleteAsync(int id);

		Task<List<Employee>> FilterAsync(Func<Employee, bool> predicate);

		Task SaveChangesAsync();
	}
}