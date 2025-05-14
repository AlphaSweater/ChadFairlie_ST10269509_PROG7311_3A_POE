using WebApp.Models;

namespace WebApp.Repositories
{
	public interface IProductRepository
	{
		Task<List<Product>> GetAllAsync();

		Task<Product?> GetByIdAsync(int id);

		Task AddAsync(Product product);

		Task UpdateAsync(Product product);

		Task DeleteAsync(int id);

		Task<List<Product>> FilterAsync(Func<Product, bool> predicate);

		Task SaveChangesAsync();
	}
}