using System.Linq.Expressions;
using WebApp.Models;

namespace WebApp.Repositories
{
	public interface IProductRepository : IBaseRepository<Product>
	{
		Task<List<Category>> GetAllCategories();
	}
}