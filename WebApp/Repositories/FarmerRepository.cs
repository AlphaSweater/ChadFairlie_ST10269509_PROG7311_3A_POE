using Microsoft.EntityFrameworkCore;
using WebApp.Models;

namespace WebApp.Repositories
{
	public class FarmerRepository : IFarmerRepository
	{
		private readonly AgriDbContext _context;

		public FarmerRepository(AgriDbContext context)
		{
			_context = context;
		}

		public async Task<List<Farmer>> GetAllAsync()
		{
			return await _context.Farmers
				.Where(f => !f.IsDeleted)
				.Include(f => f.CreatedByEmployee) // Eagerly load the Employee who created the Farmer
				.ToListAsync();
		}

		public async Task<Farmer?> GetByIdAsync(int id)
		{
			return await _context.Farmers
				.Where(f => f.FarmerId == id && !f.IsDeleted)
				.Include(f => f.CreatedByEmployee) // Eagerly load the Employee who created the Farmer
				.FirstOrDefaultAsync();
		}

		public async Task AddAsync(Farmer farmer)
		{
			await _context.Farmers.AddAsync(farmer);
		}

		public async Task UpdateAsync(Farmer farmer)
		{
			var existingFarmer = await _context.Farmers.FindAsync(farmer.FarmerId);
			if (existingFarmer != null && !existingFarmer.IsDeleted)
			{
				_context.Entry(existingFarmer).CurrentValues.SetValues(farmer);
				existingFarmer.UpdatedOn = DateTime.UtcNow;
			}
		}

		public async Task DeleteAsync(int id)
		{
			var farmer = await _context.Farmers.FindAsync(id);
			if (farmer != null && !farmer.IsDeleted)
			{
				farmer.IsDeleted = true;
				farmer.UpdatedOn = DateTime.UtcNow;
			}
		}

		public async Task<List<Farmer>> FilterAsync(Func<Farmer, bool> predicate)
		{
			return await Task.FromResult(
				_context.Farmers
					.Where(f => !f.IsDeleted)
					.Include(f => f.CreatedByEmployee) // Eagerly load the Employee who created the Farmer
					.AsEnumerable()
					.Where(predicate)
					.ToList()
			);
		}

		public async Task SaveChangesAsync()
		{
			await _context.SaveChangesAsync();
		}
	}
}