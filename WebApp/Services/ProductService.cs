using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services
{
	// Services/ProductService.cs
	public class ProductService
	{
		private readonly IProductRepository _repository;

		public ProductService(IProductRepository repository)
		{
			_repository = repository;
		}

		public Task<List<Product>> GetAllProductsAsync()
		{
			return _repository.GetAllAsync();
		}

		public Task<List<Product>> GetAllProductsByFarmerIdAsync(int farmerId)
		{
			return _repository.FilterAsync(p => p.FarmerId == farmerId);
		}

		public Task<Product?> GetProductAsync(int id)
		{
			return _repository.GetByIdAsync(id);
		}

		public async Task AddProductAsync(Product product)
		{
			product.CreatedOn = DateTime.UtcNow;
			await _repository.AddAsync(product);
			await _repository.SaveChangesAsync();
		}

		public async Task UpdateProductAsync(Product product)
		{
			await _repository.UpdateAsync(product);
			await _repository.SaveChangesAsync();
		}

		public async Task DeleteProductAsync(int id)
		{
			await _repository.DeleteAsync(id);
			await _repository.SaveChangesAsync();
		}

		public async Task<List<Product>> FilterProductsAsync(string category, double? minPrice, double? maxPrice)
		{
			return await _repository.FilterAsync(p =>
				(string.IsNullOrEmpty(category) || p.Category == category) &&
				(!minPrice.HasValue || p.Price >= minPrice.Value) &&
				(!maxPrice.HasValue || p.Price <= maxPrice.Value));
		}
	}
}