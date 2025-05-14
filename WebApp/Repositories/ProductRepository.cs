using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly AgriDbContext _context;

		public ProductRepository(AgriDbContext context)
		{
			_context = context;
		}

		public async Task<List<Product>> GetAllAsync()
		{
			return await _context.Products
				.Where(p => !p.IsDeleted)
				.Include(p => p.Farmer) // Eagerly load the Farmer navigation property
				.ThenInclude(f => f.CreatedByEmployee) // Eagerly load the Employee who created the Farmer
				.ToListAsync();
		}

		public async Task<Product?> GetByIdAsync(int id)
		{
			return await _context.Products
				.Where(p => p.ProductId == id && !p.IsDeleted)
				.Include(p => p.Farmer) // Eagerly load the Farmer navigation property
				.ThenInclude(f => f.CreatedByEmployee) // Eagerly load the Employee who created the Farmer
				.FirstOrDefaultAsync();
		}

		public async Task AddAsync(Product product)
		{
			await _context.Products.AddAsync(product);
		}

		public async Task UpdateAsync(Product product)
		{
			var existingProduct = await _context.Products.FindAsync(product.ProductId);
			if (existingProduct != null && !existingProduct.IsDeleted)
			{
				_context.Entry(existingProduct).CurrentValues.SetValues(product);
				existingProduct.UpdatedOn = DateTime.UtcNow;
			}
		}

		public async Task DeleteAsync(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product != null && !product.IsDeleted)
			{
				product.IsDeleted = true;
				product.UpdatedOn = DateTime.UtcNow;
			}
		}

		public async Task<List<Product>> FilterAsync(Func<Product, bool> predicate)
		{
			return await Task.FromResult(
				_context.Products
					.Where(p => !p.IsDeleted)
					.Include(p => p.Farmer) // Eagerly load the Farmer navigation property
					.ThenInclude(f => f.CreatedByEmployee) // Eagerly load the Employee who created the Farmer
					.AsEnumerable()
					.Where(predicate)
					.ToList()
			);
		}

		public async Task<List<Category>> GetAllCategories()
		{
			return await _context.Categories.ToListAsync();
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}