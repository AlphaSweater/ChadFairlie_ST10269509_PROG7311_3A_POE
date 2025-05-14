using WebApp.Models;

namespace WebApp.Repositories
{
	public interface IFarmerRepository : IBaseRepository<Farmer>
	{
		Task<Farmer?> GetByEmailAsync(string email);
	}
}