using WebApp.Models;
using WebApp.Repositories;

namespace WebApp.Services
{
	public class FarmerService
	{
		private readonly IFarmerRepository _repository;

		public FarmerService(IFarmerRepository repository)
		{
			_repository = repository;
		}

		public Task<List<Farmer>> GetAllFarmersAsync()
		{
			return _repository.GetAllAsync();
		}

		public Task<Farmer?> GetFarmerByIdAsync(int id)
		{
			return _repository.GetByIdAsync(id);
		}

		public Task<Farmer?> GetFarmerByEmailAsync(string email)
		{
			return _repository.GetByEmailAsync(email);
		}

		public async Task AddFarmerAsync(Farmer farmer)
		{
			farmer.CreatedOn = DateTime.UtcNow;
			await _repository.AddAsync(farmer);
			await _repository.SaveChangesAsync();
		}

		public async Task UpdateFarmerAsync(Farmer farmer)
		{
			await _repository.UpdateAsync(farmer);
			await _repository.SaveChangesAsync();
		}

		public async Task DeleteFarmerAsync(int id)
		{
			await _repository.DeleteAsync(id);
			await _repository.SaveChangesAsync();
		}

		public async Task<List<Farmer>> FilterFarmersAsync(string? name, int? createdByEmployeeId)
		{
			return await _repository.FilterAsync(f =>
				(string.IsNullOrEmpty(name) || (f.FirstName + " " + f.LastName).Contains(name, StringComparison.OrdinalIgnoreCase)) &&
				(!createdByEmployeeId.HasValue || f.CreatedByEmployeeId == createdByEmployeeId.Value));
		}
	}
}