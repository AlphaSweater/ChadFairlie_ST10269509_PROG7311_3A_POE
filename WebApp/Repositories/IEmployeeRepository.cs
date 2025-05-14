using System.Linq.Expressions;
using WebApp.Models;

namespace WebApp.Repositories
{
	public interface IEmployeeRepository : IBaseRepository<Employee>
	{
		Task<Employee?> GetByEmailAsync(string email);
	}
}