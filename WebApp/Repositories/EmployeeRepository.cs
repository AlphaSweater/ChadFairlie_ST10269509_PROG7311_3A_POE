using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Repositories
{
	public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
	{
		public EmployeeRepository(AgriDbContext context) : base(context)
		{
		}

		public async Task<Employee?> GetByEmailAsync(string email)
		{
			return await _dbSet
				.FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower() && !e.IsDeleted);
		}

		protected override string GetIdPropertyName() => "EmployeeId";
	}
}