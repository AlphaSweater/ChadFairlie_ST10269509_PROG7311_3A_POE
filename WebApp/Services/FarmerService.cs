using Microsoft.Extensions.Logging;
using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services
{
	/// <summary>
	/// Service for managing farmer-related operations
	/// </summary>
	public class FarmerService : IBaseService<Farmer, int>
	{
		private readonly IFarmerRepository _repository;
		private readonly ILogger<FarmerService> _logger;

		public FarmerService(IFarmerRepository repository, ILogger<FarmerService> logger)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		/// <inheritdoc/>
		public async Task<List<Farmer>> GetAllAsync()
		{
			try
			{
				return await _repository.GetAllAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving all farmers");
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task<Farmer?> GetByIdAsync(int id)
		{
			try
			{
				if (id <= 0)
					throw new ArgumentException("Farmer ID must be positive", nameof(id));

				return await _repository.GetByIdAsync(id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving farmer {FarmerId}", id);
				throw;
			}
		}

		/// <summary>
		/// Gets a farmer by their email address
		/// </summary>
		public async Task<Farmer?> GetByEmailAsync(string email)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(email))
					throw new ArgumentException("Email is required", nameof(email));

				return await _repository.GetByEmailAsync(email);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving farmer by email {Email}", email);
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task<Farmer> AddAsync(Farmer farmer)
		{
			try
			{
				if (farmer == null)
					throw new ArgumentNullException(nameof(farmer));

				ValidateFarmer(farmer);

				farmer.CreatedOn = DateTime.UtcNow;
				await _repository.AddAsync(farmer);
				await _repository.SaveChangesAsync();

				_logger.LogInformation("Farmer {FarmerId} created successfully", farmer.FarmerId);
				return farmer;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error creating farmer");
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task<Farmer> UpdateAsync(Farmer farmer)
		{
			try
			{
				if (farmer == null)
					throw new ArgumentNullException(nameof(farmer));

				ValidateFarmer(farmer);

				await _repository.Update(farmer);
				await _repository.SaveChangesAsync();

				_logger.LogInformation("Farmer {FarmerId} updated successfully", farmer.FarmerId);
				return farmer;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating farmer {FarmerId}", farmer?.FarmerId);
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task DeleteAsync(int id)
		{
			try
			{
				if (id <= 0)
					throw new ArgumentException("Farmer ID must be positive", nameof(id));

				await _repository.DeleteAsync(id);
				await _repository.SaveChangesAsync();

				_logger.LogInformation("Farmer {FarmerId} deleted successfully", id);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deleting farmer {FarmerId}", id);
				throw;
			}
		}

		/// <summary>
		/// Filters farmers based on name and employee ID
		/// </summary>
		public async Task<List<Farmer>> FilterFarmersAsync(string? name = null, int? createdByEmployeeId = null)
		{
			try
			{
				return await _repository.FindAsync(f =>
					(string.IsNullOrEmpty(name) || 
					 (f.FirstName + " " + f.LastName).Contains(name, StringComparison.OrdinalIgnoreCase)) &&
					(!createdByEmployeeId.HasValue || f.CreatedByEmployeeId == createdByEmployeeId.Value));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error filtering farmers");
				throw;
			}
		}

		private static void ValidateFarmer(Farmer farmer)
		{
			if (string.IsNullOrWhiteSpace(farmer.FirstName))
				throw new ArgumentException("First name is required", nameof(farmer));

			if (string.IsNullOrWhiteSpace(farmer.LastName))
				throw new ArgumentException("Last name is required", nameof(farmer));

			if (string.IsNullOrWhiteSpace(farmer.Email))
				throw new ArgumentException("Email is required", nameof(farmer));

			if (string.IsNullOrWhiteSpace(farmer.PasswordHash))
				throw new ArgumentException("Password hash is required", nameof(farmer));
		}
	}
}