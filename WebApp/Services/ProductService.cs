using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services
{
	/// <summary>
	/// Service for managing product-related operations
	/// </summary>
	public class ProductService : IBaseService<Product, int>
	{
		private readonly IProductRepository _repository;
		private readonly ILogger<ProductService> _logger;

		public ProductService(IProductRepository repository, ILogger<ProductService> logger)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <inheritdoc/>
		public async Task<List<Product>> GetAllAsync()
		{
			try
			{
				return await _repository.GetAllAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving all products");
				throw;
			}
		}

		/// <summary>
		/// Gets all products for a specific farmer
		/// </summary>
		/// <param name="farmerId">The ID of the farmer</param>
		/// <returns>A list of products belonging to the farmer</returns>
		public async Task<List<Product>> GetAllByFarmerIdAsync(int farmerId)
		{
			try
			{
				if (farmerId <= 0)
					throw new ArgumentException("Farmer ID must be positive", nameof(farmerId));

				return await _repository.FilterAsync(p => p.FarmerId == farmerId);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving products for farmer {FarmerId}", farmerId);
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task<Product?> GetByIdAsync(int id)
		{
			try
			{
				if (id <= 0)
					throw new ArgumentException("Product ID must be positive", nameof(id));

				return await _repository.GetByIdAsync(id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving product {ProductId}", id);
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task<Product> AddAsync(Product product)
		{
			try
			{
				if (product == null)
					throw new ArgumentNullException(nameof(product));

				ValidateProduct(product);

				product.CreatedOn = DateTime.UtcNow;
				await _repository.AddAsync(product);
				await _repository.SaveChangesAsync();

				_logger.LogInformation("Product {ProductId} created successfully", product.ProductId);
				return product;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating product");
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task<Product> UpdateAsync(Product product)
		{
			try
			{
				if (product == null)
					throw new ArgumentNullException(nameof(product));

				ValidateProduct(product);

				await _repository.UpdateAsync(product);
				await _repository.SaveChangesAsync();

				_logger.LogInformation("Product {ProductId} updated successfully", product.ProductId);
				return product;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating product {ProductId}", product?.ProductId);
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task DeleteAsync(int id)
		{
			try
			{
				if (id <= 0)
					throw new ArgumentException("Product ID must be positive", nameof(id));

				await _repository.DeleteAsync(id);
				await _repository.SaveChangesAsync();

				_logger.LogInformation("Product {ProductId} deleted successfully", id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting product {ProductId}", id);
				throw;
			}
		}

		/// <summary>
		/// Filters products based on category and price range
		/// </summary>
		public async Task<List<Product>> FilterProductsAsync(string? category = null, double? minPrice = null, double? maxPrice = null)
		{
			try
			{
				if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
					throw new ArgumentException("Minimum price cannot be greater than maximum price");

				return await _repository.FilterAsync(p =>
					(string.IsNullOrEmpty(category) || p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)) &&
					(!minPrice.HasValue || p.Price >= minPrice.Value) &&
					(!maxPrice.HasValue || p.Price <= maxPrice.Value));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error filtering products");
				throw;
			}
		}

		/// <summary>
		/// Gets all available product categories
		/// </summary>
		public async Task<List<Category>> GetAllCategoriesAsync()
		{
			try
			{
				var categories = await _repository.GetAllCategories();
				return categories.Select(c => new Category { Name = c.Name }).ToList();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving product categories");
				throw;
			}
		}

		private static void ValidateProduct(Product product)
		{
			if (string.IsNullOrWhiteSpace(product.Name))
				throw new ArgumentException("Product name is required", nameof(product));

			if (string.IsNullOrWhiteSpace(product.Category))
				throw new ArgumentException("Product category is required", nameof(product));

			if (product.Price < 0)
				throw new ArgumentException("Product price cannot be negative", nameof(product));

			if (product.FarmerId <= 0)
				throw new ArgumentException("Valid farmer ID is required", nameof(product));
		}
	}
}