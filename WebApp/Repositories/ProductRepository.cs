using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApp.Models;

namespace WebApp.Repositories
{
	public class ProductRepository : BaseRepository<Product>, IProductRepository
	{
		public ProductRepository(AgriDbContext context) : base(context)
		{
		}

		public override async Task<List<Product>> GetAllAsync(params Expression<Func<Product, object>>[] includes)
		{
			return await base.GetAllAsync(
				p => p.Farmer,
				p => p.Farmer.CreatedByEmployee
			);
		}

		public override async Task<Product?> GetByIdAsync(int id, params Expression<Func<Product, object>>[] includes)
		{
			return await base.GetByIdAsync(id,
				p => p.Farmer,
				p => p.Farmer.CreatedByEmployee
			);
		}

		public async Task<List<Category>> GetAllCategories()
		{
			return await _context.Categories.ToListAsync();
		}

		protected override string GetIdPropertyName() => "ProductId";

		public override async Task AddAsync(Product product)
		{
			await _context.Products.AddAsync(product);
		}

		public override async Task Update(Product product)
		{
			var existingProduct = await _context.Products.FindAsync(product.ProductId);
			if (existingProduct != null && !existingProduct.IsDeleted)
			{
				_context.Entry(existingProduct).CurrentValues.SetValues(product);
				existingProduct.UpdatedOn = DateTime.UtcNow;
			}
		}

		public override async Task DeleteAsync(int id)
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

		public override async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}