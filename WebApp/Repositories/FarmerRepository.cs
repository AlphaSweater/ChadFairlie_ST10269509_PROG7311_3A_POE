using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApp.Models;

namespace WebApp.Repositories
{
	public class FarmerRepository : BaseRepository<Farmer>, IFarmerRepository
	{
		public FarmerRepository(AgriDbContext context) : base(context)
		{
		}

		public override async Task<List<Farmer>> GetAllAsync(params Expression<Func<Farmer, object>>[] includes)
		{
			return await base.GetAllAsync(f => f.CreatedByEmployee);
		}

		public override async Task<Farmer?> GetByIdAsync(int id, params Expression<Func<Farmer, object>>[] includes)
		{
			return await base.GetByIdAsync(id, f => f.CreatedByEmployee);
		}

		public async Task<Farmer?> GetByEmailAsync(string email)
		{
			return await _dbSet
				.Include(f => f.CreatedByEmployee)
				.FirstOrDefaultAsync(f => f.Email.ToLower() == email.ToLower() && !f.IsDeleted);
		}

		protected override string GetIdPropertyName() => "FarmerId";

		public override async Task AddAsync(Farmer farmer)
		{
			await _dbSet.AddAsync(farmer);
		}

		public override async Task Update(Farmer farmer)
		{
			var existingFarmer = await _dbSet.FindAsync(farmer.FarmerId);
			if (existingFarmer != null && !existingFarmer.IsDeleted)
			{
				_context.Entry(existingFarmer).CurrentValues.SetValues(farmer);
				existingFarmer.UpdatedOn = DateTime.UtcNow;
			}
		}

		public override async Task DeleteAsync(int id)
		{
			var farmer = await _dbSet.FindAsync(id);
			if (farmer != null && !farmer.IsDeleted)
			{
				farmer.IsDeleted = true;
				farmer.UpdatedOn = DateTime.UtcNow;
			}
		}

		public async Task<List<Farmer>> FilterAsync(Func<Farmer, bool> predicate)
		{
			return await Task.FromResult(
				_dbSet
					.Where(f => !f.IsDeleted)
					.Include(f => f.CreatedByEmployee)
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