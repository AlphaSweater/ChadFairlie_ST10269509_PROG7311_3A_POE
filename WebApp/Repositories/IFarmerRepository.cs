using WebApp.Models;

namespace WebApp.Repositories
{
	public interface IFarmerRepository
	{
		Task<List<Farmer>> GetAllAsync();

		Task<Farmer?> GetByIdAsync(int id);

		Task AddAsync(Farmer farmer);

		Task UpdateAsync(Farmer farmer);

		Task DeleteAsync(int id);

		Task<List<Farmer>> FilterAsync(Func<Farmer, bool> predicate);

		Task SaveChangesAsync();
	}
}